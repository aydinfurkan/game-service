using System.Collections.Concurrent;
using System.Net.Sockets;
using AsyncAwaitBestPractices;
using GameService.Contract.ResponseModels;
using GameService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace GameService.TcpServer.Entities;

public class Server
{
    private readonly ILogger<Server> _logger;
    private readonly TcpListener _listener;
    private readonly ConcurrentDictionary<Guid, Client> _gameClientList;
    
    public Server(ILogger<Server> logger, TcpListener tcpListener)
    {
        _logger = logger;
        _listener = tcpListener;
        _gameClientList = new ConcurrentDictionary<Guid, Client>();
    }
    
    public async Task InitAsync(Func<TcpClient, Task> onClientAcceptedAsync, CancellationToken cancellationToken)
    {
        _logger.LogInformation("TcpServer started");
        _listener.Start();
        
        await StartAcceptClientAsync(onClientAcceptedAsync, cancellationToken);
        
        _listener.Stop();
        _logger.LogInformation("TcpServer stopped");
    }
    
    private async Task StartAcceptClientAsync(Func<TcpClient, Task> onClientAcceptedAsync, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var tcpClient = await _listener.AcceptTcpClientAsync(cancellationToken);
            
            onClientAcceptedAsync(tcpClient).SafeFireAndForget();
        }
    }

    public bool AddClient(Client client)
    {
        _logger.LogInformation("Client added to server");
        return _gameClientList.TryAdd(client.Id, client);
    }
    
    public void RemoveClient(Client client)
    {
        _logger.LogInformation("Client removed from server");
        _gameClientList.TryRemove(client.Id, out _);
    }
    
    public void PushGameQueues(ResponseModelData response, Func<KeyValuePair<Guid, Client>,bool>? expression = null)
    {
        _gameClientList.Where(expression ?? (_ => true)).ToList().ForEach(x => x.Value.WriteAsync(response).SafeFireAndForget());
    }
    
    public void CancelFormerConnection(User user)
    {
        KeyValuePair<Guid, Client>? gameClient = _gameClientList.FirstOrDefault(x => x.Value.User?.Id == user.Id);
        gameClient?.Value?.CancellationTokenSource.Cancel();
    }
}
