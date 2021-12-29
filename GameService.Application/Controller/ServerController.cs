using System;
using System.Threading;
using System.Threading.Tasks;
using GameService.Commands;
using GameService.Infrastructure.Logger;
using GameService.Queries;

namespace GameService.Controller
{
    public class ServerController
    {
        private readonly IPLogger<ServerController> _logger;
        private readonly GameServer _gameServer;
        private readonly GameQuery _gameQuery;
        private readonly GameCommand _gameCommand;

        public ServerController(IPLogger<ServerController> logger, GameServer gameServer, GameQuery gameQuery, GameCommand gameCommand)
        {
            _logger = logger;
            _gameServer = gameServer;
            _gameQuery = gameQuery;
            _gameCommand = gameCommand;
        }

        public void Init(CancellationToken cancellationToken)
        {
            _gameServer.Start();
            _logger.LogInformation(EventId.ServerController, "TcpServer started.");
            
            Task.Run(LogThreadCountAsync, cancellationToken);
            
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation(EventId.ServerController, "TcpServer waiting for accept a client.");
                    var gameClientTask = Task.Run(async () => await _gameServer.AcceptClient(), cancellationToken);
                    var gameClient = gameClientTask.Result;
                    if (gameClient == null) continue;
                    Task.Run(() => NewConnection(gameClient), cancellationToken);
                }
                catch(Exception exception)
                {
                    _logger.LogError(EventId.ServerController, "TcpServer exception.", exception);
                }
            }
            
            _gameServer.Stop();
            _logger.LogInformation(EventId.ServerController, "TcpServer stopped.");
        }


        private void NewConnection(GameClient gameClient)
        {
            _gameCommand.SetCharacterActive(gameClient.Character);

            var streamTask = new Task(() => StreamClient(gameClient, gameClient.Character.CharacterId),
                gameClient.CancellationTokenSource.Token);
            var subscribeTask = new Task(() => SubscribeClient(gameClient, gameClient.Character.CharacterId),
                gameClient.CancellationTokenSource.Token);
            try
            {
                streamTask.Start();
                subscribeTask.Start();
                _logger.LogInformation(gameClient.CorrelationId, "Stream and Subscribe tasks are started.");
                Task.WaitAny(new[] {streamTask, subscribeTask}, gameClient.CancellationTokenSource.Token);
            }
            catch(Exception e)
            {
                _logger.LogError(gameClient.CorrelationId,"Client disconnected.", e);
            }
            finally
            {
                _gameCommand.SetCharacterDeactivated(gameClient.Character); 
                _gameServer.CloseClient(gameClient);
                streamTask.Dispose();
                subscribeTask.Dispose();
                _logger.LogInformation(gameClient.CorrelationId, "Stream and Subscribe tasks are disposed.");
            }

        }

        private void StreamClient(GameClient gameClient, Guid id)
        {
            _logger.LogInformation(gameClient.CorrelationId,"Stream process start.");
            while (true)
            {
                var activeCharacters = _gameQuery.GetAllActiveCharacters();
                var ok = _gameServer.Write(gameClient.TcpClient, activeCharacters);
                Thread.Sleep(50);
                if (!ok) break;
            }
            _logger.LogInformation(gameClient.CorrelationId,"Stream process end.");
        }
        
        private void SubscribeClient(GameClient gameClient, Guid id)
        {
            _logger.LogInformation(gameClient.CorrelationId,"Subscribe process start.");
            while (true)
            {
                var ok = _gameServer.Read(gameClient.TcpClient, out var input);
            
                var requestModel = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestModel.RequestModel>(input);
                if (requestModel == null) continue;
            
                _gameCommand.ChangeCharacterPosition(id, requestModel.Position.ToDomainModel());
                _gameCommand.ChangeCharacterQuaternion(id, requestModel.Quaternion.ToDomainModel());
                _gameCommand.ChangeMoveState(id, requestModel.MoveState);
                _gameCommand.ChangeJumpState(id, requestModel.JumpState);
                if (!ok) break;
            }
            _logger.LogInformation(gameClient.CorrelationId,"Subscribe process end.");
        }
        
        private async Task LogThreadCountAsync()
        {
            while(true)
            {
                await Task.Delay(10000);
                _logger.LogInformation(EventId.ThreadCount,$"Working thread count : {System.Diagnostics.Process.GetCurrentProcess().Threads.Count}");
            }
        }
    }
}