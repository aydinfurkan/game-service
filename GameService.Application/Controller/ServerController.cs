using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using GameService.Commands;
using GameService.Domain.Abstracts.AntiCorruption;
using GameService.Queries;
using Microsoft.Extensions.Logging;

namespace GameService.Controller
{
    public class ServerController
    {
        private readonly ILogger _logger;
        private readonly TcpServer _tcpServer;
        private readonly IUserAntiCorruption _userAntiCorruption;
        private readonly GameQuery _gameQuery;
        private readonly GameCommand _gameCommand;

        public ServerController(ILogger logger, TcpServer tcpServer, GameQuery gameQuery, GameCommand gameCommand, IUserAntiCorruption userAntiCorruption)
        {
            _logger = logger;
            _tcpServer = tcpServer;
            _gameQuery = gameQuery;
            _gameCommand = gameCommand;
            _userAntiCorruption = userAntiCorruption;
        }

        public void Init(CancellationToken cancellationToken)
        {
            _tcpServer.Start();
            _logger.LogInformation("TcpServer started.");
            
            while (!cancellationToken.IsCancellationRequested)
            {
                var client = _tcpServer.AcceptClient();
                Task.Run(async () => await NewConnection(client), cancellationToken);
            }
            
            _tcpServer.Stop();
            _logger.LogInformation("TcpServer stopped.");
        }

        private async Task NewConnection(TcpClient client)
        {
            if (_tcpServer.HandShake(client))
            {
                var ok = _tcpServer.Read(client, out var pToken);
                var character = await _userAntiCorruption.VerifyUser(pToken);
                if (character != null) 
                {
                    _gameCommand.SetCharacterActive(character);

                    var streamTask = new Task(() => StreamClient(client, character.Id));
                    var subscribeTask = new Task(() => SubscribeClient(client, character.Id));

                    streamTask.Start();
                    subscribeTask.Start();

                    var index = Task.WaitAny(streamTask);

                    _gameCommand.SetCharacterDeactivated(character);

                    _logger.LogInformation(
                        $"Thread : {Thread.CurrentThread.ManagedThreadId} --- Client Disconnected ({index}) : {client.Client.RemoteEndPoint}");
                    streamTask.Dispose();
                    subscribeTask.Dispose();
                }
            }
            client.Close();
            _logger.LogInformation($"Thread : {Thread.CurrentThread.ManagedThreadId} is closing.");
        }

        private void StreamClient(TcpClient client, Guid id)
        {
            _logger.LogInformation($"Thread : {Thread.CurrentThread.ManagedThreadId} --- Stream process start : {client.Client.RemoteEndPoint}");
            while (true)
            {
                var activeCharacters = _gameQuery.GetAllActiveCharacters();
                var ok = _tcpServer.Write(client, activeCharacters);
                Thread.Sleep(50);
                if (!ok) break;
            }
            _logger.LogInformation($"Thread : {Thread.CurrentThread.ManagedThreadId} --- Stream process end : {client.Client.RemoteEndPoint}");
        }
        
        private void SubscribeClient(TcpClient client, Guid id)
        {
            _logger.LogInformation($"Thread : {Thread.CurrentThread.ManagedThreadId} --- Subscribe process start : {client.Client.RemoteEndPoint}");
            while (true)
            {
                var ok = _tcpServer.Read(client, out var input);
            
                var requestModel = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestModel.RequestModel>(input);
                if (requestModel == null) continue;
            
                _gameCommand.ChangeCharacterPosition(id, requestModel.Position.ToDomainModel());
                _gameCommand.ChangeCharacterQuaternion(id, requestModel.Quaternion.ToDomainModel());
                if (!ok) break;
            }
            _logger.LogInformation($"Thread : {Thread.CurrentThread.ManagedThreadId} --- Subscribe process end : {client.Client.RemoteEndPoint}");
        }
    }
}