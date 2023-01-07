using System.Collections.Concurrent;
using System.Net.Sockets;
using AsyncAwaitBestPractices;
using GameService.Contract.ResponseModels;
using GameService.Domain.Entities;
using GameService.TcpServer.Infrastructure.Protocol;
using Microsoft.Extensions.Logging;

namespace GameService.TcpServer.Controllers;

public class Server
{
    private readonly ILogger<Server> _logger;
    private readonly TcpListener _listener;
    private readonly IProtocol _protocol;
    private readonly ConcurrentDictionary<Guid, Client> _gameClientList;
    private readonly IServiceProvider _serviceProvider;
    

    public Server(ILogger<Server> logger, TcpListener tcpListener, IProtocol protocol, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _listener = tcpListener;
        _protocol = protocol;
        _serviceProvider = serviceProvider;
        _gameClientList = new ConcurrentDictionary<Guid, Client>();
    }
    
    public async Task InitAsync(Func<Client, Task> onClientAcceptedAsync, CancellationToken cancellationToken)
    {
        _logger.LogInformation("TcpServer started.");
        _listener.Start();
        
        await StartAcceptClientAsync(onClientAcceptedAsync, cancellationToken);
        
        _listener.Stop();
        _logger.LogInformation("TcpServer stopped.");
    }
    
    private async Task StartAcceptClientAsync(Func<Client, Task> onClientAcceptedAsync, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var tcpClient = await _listener.AcceptTcpClientAsync(cancellationToken);

            onClientAcceptedAsync(new Client(tcpClient, _serviceProvider)).SafeFireAndForget();
        }
    }

    public bool AddClient(Client client)
    {
        return _gameClientList.TryAdd(client.Id, client);
    }
    
    public void CloseClient(Client client)
    {
        client.Close();
        while (!_gameClientList.TryRemove(client.Id, out _)){}
    }
    
    public void PushGameQueues(ResponseModelData response, Func<KeyValuePair<Guid, Client>,bool>? expression = null)
    {
        _gameClientList.Where(expression ?? (_ => true)).ToList().ForEach(x => x.Value.WriteAsync(response).SafeFireAndForget());
    }
    
    public void CancelFormerConnection(User user)
    {
        var gameClient = _gameClientList.FirstOrDefault(x => x.Value.User?.Id == user.Id);
        gameClient.Value.CancellationTokenSource.Cancel();

        // var stopwatch = new Stopwatch();
        // stopwatch.Start();
        // while (gameClient is { IsActive: true })
        // {
        //     if (stopwatch.ElapsedMilliseconds > 5000)
        //     {
        //         throw new Exception("Cancel former connection timeout.");
        //     }
        // }
    }
}
