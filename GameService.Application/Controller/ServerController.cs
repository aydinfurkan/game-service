using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using GameService.Commands;
using GameService.Domain.Abstracts.AntiCorruption;
using GameService.Domain.Entity;
using GameService.Domain.ValueObject;
using GameService.Queries;
using Microsoft.Extensions.Logging;

namespace GameService.Controller
{
    public class ServerController
    {
        private readonly ILogger _logger;
        private readonly GameServer _gameServer;
        private readonly IUserAntiCorruption _userAntiCorruption;
        private readonly GameQuery _gameQuery;
        private readonly GameCommand _gameCommand;

        public ServerController(ILogger logger, GameServer gameServer, GameQuery gameQuery, GameCommand gameCommand, IUserAntiCorruption userAntiCorruption)
        {
            _logger = logger;
            _gameServer = gameServer;
            _gameQuery = gameQuery;
            _gameCommand = gameCommand;
            _userAntiCorruption = userAntiCorruption;
        }

        public async Task Init(CancellationToken cancellationToken)
        {
            _gameServer.Start();
            _logger.LogInformation("TcpServer started.");
            
            while (!cancellationToken.IsCancellationRequested)
            {
                var gameClient = await _gameServer.AcceptClient();
                if (gameClient != null)
                    Task.Run(() => NewConnection(gameClient), cancellationToken);
            }
            
            _gameServer.Stop();
            _logger.LogInformation("TcpServer stopped.");
        }

        private void NewConnection(GameClient gameClient)
        {
            _gameCommand.SetCharacterActive(gameClient.Character);
            
            var streamTask = new Task(() => StreamClient(gameClient, gameClient.Character.CharacterId));
            var subscribeTask = new Task(() => SubscribeClient(gameClient, gameClient.Character.CharacterId));

            streamTask.Start();
            subscribeTask.Start();

            var index = Task.WaitAny(streamTask);

            _gameCommand.SetCharacterDeactivated(gameClient.Character);

            _logger.LogInformation($"Thread : {Thread.CurrentThread.ManagedThreadId} --- Client Disconnected ({index}) : {gameClient.TcpClient.Client.RemoteEndPoint}");
            
            streamTask.Dispose();
            subscribeTask.Dispose();
            _gameServer.CloseClient(gameClient);
            _logger.LogInformation($"Thread : {Thread.CurrentThread.ManagedThreadId} is closing.");
        }

        private void StreamClient(GameClient gameClient, Guid id)
        {
            _logger.LogInformation($"Thread : {Thread.CurrentThread.ManagedThreadId} --- Stream process start : {gameClient.TcpClient.Client.RemoteEndPoint}");
            while (true)
            {
                var activeCharacters = _gameQuery.GetAllActiveCharacters();
                var ok = _gameServer.Write(gameClient.TcpClient, activeCharacters);
                Thread.Sleep(50);
                if (!ok) break;
            }
            _logger.LogInformation($"Thread : {Thread.CurrentThread.ManagedThreadId} --- Stream process end : {gameClient.TcpClient.Client.RemoteEndPoint}");
        }
        
        private void SubscribeClient(GameClient gameClient, Guid id)
        {
            _logger.LogInformation($"Thread : {Thread.CurrentThread.ManagedThreadId} --- Subscribe process start : {gameClient.TcpClient.Client.RemoteEndPoint}");
            while (true)
            {
                var ok = _gameServer.Read(gameClient.TcpClient, out var input);
            
                var requestModel = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestModel.RequestModel>(input);
                if (requestModel == null) continue;
            
                _gameCommand.ChangeCharacterPosition(id, requestModel.Position.ToDomainModel());
                _gameCommand.ChangeCharacterQuaternion(id, requestModel.Quaternion.ToDomainModel());
                if (!ok) break;
            }
            _logger.LogInformation($"Thread : {Thread.CurrentThread.ManagedThreadId} --- Subscribe process end : {gameClient.TcpClient.Client.RemoteEndPoint}");
        }
    }
}