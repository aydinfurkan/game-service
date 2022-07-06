
using System.Net.Sockets;
using System.Timers;
using GameService.Domain.Entities;
using GameService.Infrastructure.Logger;
using GameService.TcpServer.Abstractions;

namespace GameService.TcpServer.Controllers;

public class ServerController
{
    private readonly IPLogger<ServerController> _logger;
    private readonly Server _server;
    private readonly ICharacterController _characterController;

    public ServerController(
        IPLogger<ServerController> logger, 
        Server server,
        ICharacterController characterController)
    {
        _logger = logger;
        _server = server;
        _characterController = characterController;
    }

    public void Init(CancellationToken cancellationToken)
    {
        _server.Start();
        _logger.LogInformation(EventId.ServerController, "TcpServer started.");
            
        Task.Run(LogThreadCountAsync, cancellationToken);
        
        var game = new Game();

        while (!cancellationToken.IsCancellationRequested)
        {
            var tcpClient = Task.Run(_server.AcceptClientAsync, cancellationToken).Result;
                
            Task.Run(() => NewConnectionAsync(game, tcpClient), cancellationToken).ContinueWith(t => 
                    _logger.LogError(EventId.ServerController, "New connection exception.", t.Exception),
                TaskContinuationOptions.OnlyOnFaulted);
        }
        
        _server.Stop();
        _logger.LogInformation(EventId.ServerController, "TcpServer stopped.");
    }
        
    private async Task NewConnectionAsync(Game game, TcpClient tcpClient)
    {
        _server.OpenNewConnection(tcpClient);
        var client = new Client(tcpClient);
            
        var streamTask = Task.Run(() => StreamClient(client)).ContinueWith(t => 
                _logger.LogError(client.CorrelationId, $"Stream process exception. ({client.User.Email} - {client.Character.Id})", t.Exception),
            TaskContinuationOptions.NotOnRanToCompletion);
            
        var subscribeTask = Task.Run(() => SubscribeClient(game, client)).ContinueWith(t => 
                _logger.LogError(client.CorrelationId, $"Subscribe process exception. ({client.User.Email} - {client.Character.Id})", t.Exception),
            TaskContinuationOptions.NotOnRanToCompletion);
            
        var tickTimer = new System.Timers.Timer(500) {Enabled = true};
        tickTimer.Elapsed += (_, e) => TickClient(game, client, e);
            
        try
        {
            Task.WaitAny(new[] {streamTask, subscribeTask}, client.CancellationTokenSource.Token);
        }
        finally
        {
            await _characterController.Disconnect(game, client);
            _server.CloseClient(client);
            tickTimer.Stop();
            tickTimer.Dispose();
        }

    }

    private void StreamClient(Client client)
    {
        _logger.LogInformation(client.CorrelationId,$"Stream process start. ({client.CorrelationId})");
            
        foreach (var responseObj in client.GameQueue.GetConsumingEnumerable())
        {
            var ok = _server.Write(client.TcpClient, responseObj);
            if (!ok) break;
        }
            
        _logger.LogInformation(client.CorrelationId,$"Stream process end. ({client.CorrelationId})");
    }

    private void SubscribeClient(Game game, Client client)
    {
        _logger.LogInformation(client.CorrelationId,$"Subscribe process start. ({client.CorrelationId})");
        while (true)
        {
            var input = _server.Read(client.TcpClient);
            if (input == null) break;

            _characterController.Send(game, client, input); // TODO fire forget command
        }

        _logger.LogInformation(client.CorrelationId,$"Subscribe process end. ({client.CorrelationId})");
    }
    
    private void TickClient(Game game, Client client, ElapsedEventArgs eventArgs)
    {
        _characterController.Tick(game, client, eventArgs);
    }

    private async Task LogThreadCountAsync()
    {
        while(true)
        {
            await Task.Delay(5*60*1000);
            _logger.LogInformation(EventId.ThreadCount,$"Working thread count : {System.Diagnostics.Process.GetCurrentProcess().Threads.Count}");
        }
    }
}