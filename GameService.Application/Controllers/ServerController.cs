using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using GameService.Application.Commands;
using GameService.Application.Handler;
using GameService.Application.Queries;
using GameService.Infrastructure.Logger;
using GameService.Infrastructure.Protocol.RequestModels;

namespace GameService.Application.Controllers;

public class ServerController
{
    private readonly IPLogger<ServerController> _logger;
    private readonly GameServer _gameServer;
    private readonly GameQuery _gameQuery;
    private readonly GameCommand _gameCommand;
    private readonly UserCommand _userCommand;
    private readonly InputQueue _inputQueue;
    private readonly InputHandler _inputHandler;

    public ServerController(IPLogger<ServerController> logger, GameServer gameServer, GameQuery gameQuery, 
        GameCommand gameCommand, UserCommand userCommand, InputQueue inputQueue, InputHandler inputHandler)
    {
        _logger = logger;
        _gameServer = gameServer;
        _gameQuery = gameQuery;
        _gameCommand = gameCommand;
        _userCommand = userCommand;
        _inputQueue = inputQueue;
        _inputHandler = inputHandler;
    }

    public void Init(CancellationToken cancellationToken)
    {
        _gameServer.Start();
        _logger.LogInformation(EventId.ServerController, "TcpServer started.");
            
        Task.Run(LogThreadCountAsync, cancellationToken);
        Task.Run(HandleInputQueue, cancellationToken);
            
        while (!cancellationToken.IsCancellationRequested)
        {
            var tcpClient = Task.Run(_gameServer.AcceptClientAsync, cancellationToken).Result;
                
            Task.Run(() => NewConnectionAsync(tcpClient), cancellationToken).ContinueWith(t => 
                    _logger.LogError(EventId.ServerController, "New connection exception.", t.Exception),
                TaskContinuationOptions.OnlyOnFaulted);
        }
            
        _gameServer.Stop();
        _logger.LogInformation(EventId.ServerController, "TcpServer stopped.");
    }
        
    private async Task NewConnectionAsync(TcpClient tcpClient)
    {
        _gameServer.OpenNewConnection(tcpClient);
        var gameClient = await VerifyClient(tcpClient);
            
        var streamTask = Task.Run(() => StreamClient(gameClient)).ContinueWith(t => 
                _logger.LogError(gameClient.CorrelationId, $"Stream process exception. ({gameClient.User.Email} - {gameClient.Character.Id})", t.Exception),
            TaskContinuationOptions.NotOnRanToCompletion);
            
        var subscribeTask = Task.Run(() => SubscribeClient(gameClient)).ContinueWith(t => 
                _logger.LogError(gameClient.CorrelationId, $"Subscribe process exception. ({gameClient.User.Email} - {gameClient.Character.Id})", t.Exception),
            TaskContinuationOptions.NotOnRanToCompletion);
            
        var tickTimer = new System.Timers.Timer(500) {Enabled = true};
        tickTimer.Elapsed += (_, e) => TickClient(gameClient, e);
            
        try
        {
            Task.WaitAny(new[] {streamTask, subscribeTask}, gameClient.CancellationTokenSource.Token);
        }
        finally
        {
            await _userCommand.UpdateCharacterAsync(gameClient.PToken, gameClient.Character);
            _gameCommand.SetCharacterDeactivated(gameClient.Character);
            _inputHandler.HandleDisconnect(gameClient);
            _gameServer.CloseClient(gameClient);
            tickTimer.Stop();
            tickTimer.Dispose();
        }

    }

    private void StreamClient(GameClient gameClient)
    {
        _logger.LogInformation(gameClient.CorrelationId,$"Stream process start. ({gameClient.User.Email} - {gameClient.Character.Id})");
            
        foreach (var responseObj in gameClient.GameQueue.GetConsumingEnumerable())
        {
            var ok = _gameServer.Write(gameClient.TcpClient, responseObj);
            if (!ok) break;
        }
            
        _logger.LogInformation(gameClient.CorrelationId,$"Stream process end. ({gameClient.User.Email} - {gameClient.Character.Id})");
    }
        
    private void SubscribeClient(GameClient gameClient)
    {
        _logger.LogInformation(gameClient.CorrelationId,$"Subscribe process start. ({gameClient.User.Email} - {gameClient.Character.Id})");
        while (true)
        {
            var input = _gameServer.Read(gameClient.TcpClient);
            if (input == null) break;

            var clientInput = new ClientInput(gameClient, input);
            _inputQueue.Push(clientInput);
        }

        _logger.LogInformation(gameClient.CorrelationId,$"Subscribe process end. ({gameClient.User.Email} - {gameClient.Character.Id})");
    }
    private void TickClient(GameClient gameClient, ElapsedEventArgs eventArgs)
    {
        _inputHandler.HandleTick(gameClient, eventArgs.SignalTime);
    }
        
    private async Task<GameClient> VerifyClient(TcpClient client)
    {
        if (_gameServer.Read(client) is not VerificationModel verificationModel)
            throw new Exception("Read data is not typeof VerificationModel.");

        return await _inputHandler.HandleConnectionAsync(verificationModel, client);
    }

    private async Task LogThreadCountAsync()
    {
        while(true)
        {
            await Task.Delay(5*60*1000);
            _logger.LogInformation(EventId.ThreadCount,$"Working thread count : {System.Diagnostics.Process.GetCurrentProcess().Threads.Count}");
        }
    }
    private void HandleInputQueue()
    {
        while(true)
        {
            _inputQueue.Handle();
        }
    }
}