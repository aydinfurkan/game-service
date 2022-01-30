using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using GameService.Commands;
using GameService.Handler;
using GameService.Infrastructure.Logger;
using GameService.Infrastructure.Protocol.RequestModels;
using GameService.Queries;

namespace GameService.Controllers
{
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
            GameCommand gameCommand, UserCommand userCommand,
            InputQueue inputQueue, InputHandler inputHandler)
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
                try
                {
                    _logger.LogInformation(EventId.ServerController, "TcpServer waiting for accept a client.");
                    var tcpClient = Task.Run(_gameServer.AcceptClientAsync, cancellationToken).Result;
                    if (tcpClient == null) continue;
                    
                    Task.Run(() => NewConnectionAsync(tcpClient), cancellationToken);
                }
                catch(Exception e)
                {
                    _logger.LogError(EventId.ServerController, "TcpServer exception.", e);
                }
            }
            
            _gameServer.Stop();
            _logger.LogInformation(EventId.ServerController, "TcpServer stopped.");
        }
        
        private async Task NewConnectionAsync(TcpClient tcpClient)
        {
            var ok = await _gameServer.OpenNewConnectionAsync(tcpClient);
            var gameClient = await VerifyClient(tcpClient);
            
            var streamTask = Task.Run(() => StreamClient(gameClient), gameClient.CancellationTokenSource.Token);
            var subscribeTask = Task.Run(() => SubscribeClient(gameClient), gameClient.CancellationTokenSource.Token);
            try
            {
                Task.WaitAny(new[] {streamTask, subscribeTask}, gameClient.CancellationTokenSource.Token);
            }
            catch (OperationCanceledException e)
            {
                _logger.LogWarning(gameClient.CorrelationId, "Multi client detection. User Id : " + gameClient.User.Id);
            }
            finally
            {
                await _userCommand.UpdateCharacter(gameClient.PToken, gameClient.Character);
                _gameCommand.SetCharacterDeactivated(gameClient.Character);
                _inputHandler.HandleDisconnect(gameClient);
                _gameServer.CloseClient(gameClient);
            }

        }

        private async Task StreamClient(GameClient gameClient)
        {
            _logger.LogInformation(gameClient.CorrelationId,"Stream process start.");
            while (true)
            {
                try
                {
                    if (!gameClient.GameQueue.TryDequeue(out var responseObj)) continue;
                    
                    var ok = await _gameServer.WriteAsync(gameClient.TcpClient, responseObj);
                    if (!ok) break;
                }
                catch(Exception e)
                {
                    _logger.LogError(gameClient.CorrelationId, "Stream client exception.", e);
                    break;
                }
            }
            _logger.LogInformation(gameClient.CorrelationId,"Stream process end.");
        }
        
        private async Task SubscribeClient(GameClient gameClient)
        {
            _logger.LogInformation(gameClient.CorrelationId,"Subscribe process start.");
            while (true)
            {
                try
                {
                    var input = await _gameServer.ReadAsync(gameClient.TcpClient);
                    if (input == null) break;

                    var clientInput = new ClientInput(gameClient, input);
                    _inputQueue.Push(clientInput);

                }
                catch(Exception e)
                {
                    _logger.LogError(gameClient.CorrelationId, "Subscribe client exception.", e);
                    break;
                }
            }
            _logger.LogInformation(gameClient.CorrelationId,"Subscribe process end.");
        }
        
        private async Task<GameClient> VerifyClient(TcpClient client)
        {
            if(await _gameServer.ReadAsync(client) is not VerificationModel verificationModel) return null;

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
}