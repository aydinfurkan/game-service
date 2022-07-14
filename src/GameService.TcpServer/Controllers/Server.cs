using System.Diagnostics;
using System.Net.Sockets;
using GameService.Contract.Commands;
using GameService.Contract.ResponseModels;
using GameService.Domain.Entities;
using GameService.Infrastructure.Logger;
using GameService.TcpServer.Infrastructure.Protocol;

namespace GameService.TcpServer.Controllers;

public class Server
{
    private readonly IPLogger<Server> _logger;
    private readonly TcpListener _listener;
    private readonly IProtocol _protocol;
    private readonly List<Client> _gameClientList;

    public Server(IPLogger<Server> logger, TcpListener tcpListener, IProtocol protocol)
    {
        _logger = logger;
        _listener = tcpListener;
        _protocol = protocol;
        _gameClientList = new List<Client>();
    }
        
    public void Start()
    {
        _listener.Start();
    }
    public void Stop()
    {
        _listener.Stop();
    }
    public async Task<TcpClient> AcceptClientAsync()
    {
        return await _listener.AcceptTcpClientAsync();
    }
    public void AddClient(Client client)
    {
        _gameClientList.Add(client);
    }
    public void CloseClient(Client client)
    {
        _gameClientList.Remove(client);
        client.GameQueue.CompleteAdding();
        client.CancellationTokenSource.Cancel();
        client.TcpClient.Close();
    }
    public bool Write<T>(TcpClient tcpClient, T obj) where T : ResponseModelData
    {
        return _protocol.Write(tcpClient, obj);
    }
    public CommandBaseData? Read(TcpClient tcpClient)
    {
        return _protocol.Read(tcpClient);
    }
        
    public void PushGameQueues(ResponseModelData response, Func<Client,bool>? expression = null)
    {
        _gameClientList.Where(expression ?? (_ => true)).ToList().ForEach(x => x.GameQueue.Add(response));
    }
    public void CancelFormerConnection(User user)
    {
        var gameClient = _gameClientList.FirstOrDefault(x => x.User.Id == user.Id);
        gameClient?.CancellationTokenSource.Cancel();
            
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        while (gameClient != null && gameClient.TcpClient.Connected)
        {
            if (stopwatch.ElapsedMilliseconds > 5000)
            {
                throw new Exception("Cancel former connection timeout.");
            }
        }
    }

    public void OpenNewConnection(TcpClient tcpClient)
    {
        _protocol.HandShake(tcpClient);
    }

}
