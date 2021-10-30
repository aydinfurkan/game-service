using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using GameService.Commands;
using GameService.Controller.RequestModel;
using GameService.Queries;
using Microsoft.Extensions.Logging;

namespace GameService.Controller
{
    public class ServerController
    {
        private readonly ILogger _logger;
        private readonly TcpServer _tcpServer;
        private readonly GameQuery _gameQuery;
        private readonly GameCommand _gameCommand;

        public ServerController(ILogger logger, TcpServer tcpServer, GameQuery gameQuery, GameCommand gameCommand)
        {
            _tcpServer = tcpServer;
            _gameQuery = gameQuery;
            _gameCommand = gameCommand;
            _logger = logger;
        }

        public void Init(CancellationToken cancellationToken)
        {
            _tcpServer.Start();
            _logger.LogInformation("TcpServer started.");
            
            while (!cancellationToken.IsCancellationRequested)
            {
                var client = _tcpServer.AcceptClient();
                Task.Run(() => NewConnection(client), cancellationToken);
            }
            
            _tcpServer.Stop();
            _logger.LogInformation("TcpServer stopped.");
        }

        private void NewConnection(TcpClient client)
        {
            var input = _tcpServer.Read(client);
            if (ValidateId(input, out var id))
            {
                _gameCommand.SetPlayerActive(id);
                
                var streamTask = new Task(() => StreamClient(client, id));
                var subscribeTask = new Task(() => SubscribeClient(client, id));
                
                streamTask.Start();
                subscribeTask.Start();
                
                var index = Task.WaitAny(streamTask, subscribeTask);
                
                _gameCommand.SetPlayerDeactivated(id);

                _logger.LogInformation($"Error client : {client.Client.RemoteEndPoint}");
                streamTask.Dispose();
                subscribeTask.Dispose();
            }
            client.Close();
        }

        private bool ValidateId(string input, out Guid id)
        {
            if (!Guid.TryParse(input, out id))
            {
                _logger.LogInformation($"Thread : {Thread.CurrentThread.ManagedThreadId} --- Wrong Input");
                return false;
            }
            
            if (!_gameQuery.IsIdExist(id))
            {
                _logger.LogInformation($"Thread : {Thread.CurrentThread.ManagedThreadId} --- Id is not exist.");
                return false;
            }

            if (_gameQuery.IsPlayerActive(id))
            {
                _logger.LogInformation($"Thread : {Thread.CurrentThread.ManagedThreadId} --- Client already connected.");
                return false;
            }
            
            return true;
        }

        private void StreamClient(TcpClient client, Guid id)
        {
            while (true)
            {
                var activePlayers = _gameQuery.GetAllActivePlayers();
                var ok = _tcpServer.Write(client, activePlayers);
                Thread.Sleep(100);
                if (!ok) break;
            }
        }
        
        private void SubscribeClient(TcpClient client, Guid id)
        {
            while (true)
            {
                var input = _tcpServer.Read(client);
            
                var positionRequestModel = Newtonsoft.Json.JsonConvert.DeserializeObject<PositionRequestModel>(input);

                if (positionRequestModel == null) continue;
            
                var ok = _gameCommand.ChangePlayerPosition(id, positionRequestModel.ToDomainModel());
            }
        }
    }
}