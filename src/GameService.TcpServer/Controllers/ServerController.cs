using System.Net.Sockets;
using GameService.Domain.Entities;
using GameService.TcpServer.Abstractions;
using Microsoft.Extensions.Logging;

namespace GameService.TcpServer.Controllers;

public class ServerController
{
    private readonly ILogger<ServerController> _logger;
    private readonly Server _server;
    private readonly ICharacterController _characterController;

    public ServerController(
        ILogger<ServerController> logger, 
        Server server,
        ICharacterController characterController)
    {
        _logger = logger;
        _server = server;
        _characterController = characterController;
    }
    //
    // public async Task InitAsync(CancellationToken cancellationToken)
    // {
    //     _server.Start();
    //     
    //     await StartGameAsync(cancellationToken);
    //     
    //     _server.Stop();
    // }
    //
    // private async Task StartGameAsync(CancellationToken cancellationToken)
    // {
    //     var game = new Game();
    //
    //     while (!cancellationToken.IsCancellationRequested)
    //     {
    //         var tcpClient = await _server.AcceptClientAsync(cancellationToken);
    //
    //         NewConnectionAsync(game, tcpClient).SafeFireAndForget();
    //         
    //         // Task.Run(() => NewConnectionAsync(game, tcpClient), cancellationToken).ContinueWith(t => 
    //         //         _logger.LogError(EventId.ServerController, "New connection exception.", t.Exception),
    //         //     TaskContinuationOptions.OnlyOnFaulted);
    //     }
    // }
    //     
    private async Task NewConnectionAsync(Game game, TcpClient tcpClient)
    {
        // _server.OpenNewConnection(tcpClient);
        // var client = new Client(tcpClient);

        // StreamClientAsync(client).SafeFireAndForget();
        // SubscribeClientAsync(game, client).SafeFireAndForget();
        
            
        // var streamTask = Task.Run(() => StreamClient(client)).ContinueWith(t => 
        //         _logger.LogError(client.CorrelationId, $"Stream process exception. ({client.User.Email} - {client.Character.Id})", t.Exception),
        //     TaskContinuationOptions.NotOnRanToCompletion);
        //     
        // var subscribeTask = Task.Run(() => SubscribeClient(game, client)).ContinueWith(t => 
        //         _logger.LogError(client.CorrelationId, $"Subscribe process exception. ({client.User.Email} - {client.Character.Id})", t.Exception),
        //     TaskContinuationOptions.NotOnRanToCompletion);
            
        // var tickTimer = new System.Timers.Timer(500) {Enabled = true};
        // tickTimer.Elapsed += (_, e) => TickClient(game, client, e);
        //     
        // try
        // {
            // Task.WaitAny(new[] {streamTask, subscribeTask}, client.CancellationTokenSource.Token);
        // }
        // finally
        // {
            // await _characterController.Disconnect(game, client);
            // _server.CloseClient(client);
            // tickTimer.Stop();
            // tickTimer.Dispose();
        // }

    }

    // private async Task StreamClientAsync(Client? client)
    // {
    //     _logger.LogInformation(client.CorrelationId,$"Stream process start. ({client.CorrelationId})");
    //         
    //     foreach (var responseObj in client.GameQueue.GetConsumingEnumerable())
    //     {
    //         await _server.WriteAsync(client.TcpClient, responseObj);
    //     }
    //         
    //     _logger.LogInformation(client.CorrelationId,$"Stream process end. ({client.CorrelationId})");
    // }

    // private async Task SubscribeClientAsync(Game game, Client client)
    // {
    //     _logger.LogInformation(client.CorrelationId,$"Subscribe process start. ({client.CorrelationId})");
    //     while (true)
    //     {
    //         var input = await _server.ReadAsync(client.TcpClient);
    //         if (input == null) break;
    //
    //         _characterController.Send(game, client, input); // TODO fire forget command
    //     }
    //
    //     _logger.LogInformation(client.CorrelationId,$"Subscribe process end. ({client.CorrelationId})");
    // }
    
    // private void TickClient(Game game, Client client, ElapsedEventArgs eventArgs)
    // {
    //     _characterController.Tick(game, client, eventArgs);
    // }

}