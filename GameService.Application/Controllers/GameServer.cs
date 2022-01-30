using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using GameService.Commands;
using GameService.Domain.Entities;
using GameService.Infrastructure.Logger;
using GameService.Infrastructure.Protocol;
using GameService.Infrastructure.Protocol.RequestModels;
using GameService.Infrastructure.Protocol.ResponseModels;
using GameService.Queries;
using Character = GameService.Infrastructure.Protocol.CommonModels.Character;

namespace GameService.Controllers
{
    public class GameServer
    {
        private readonly IPLogger<GameServer> _logger;
        private readonly TcpListener _listener;
        private readonly IProtocol _protocol;
        private readonly List<GameClient> _gameClientList;
        private readonly GameCommand _gameCommand;
        private readonly GameQuery _gameQuery;

        public GameServer(IPLogger<GameServer> logger, TcpListener tcpListener, IProtocol protocol, GameCommand gameCommand, GameQuery gameQuery)
        {
            _logger = logger;
            _listener = tcpListener;
            _protocol = protocol;
            _gameCommand = gameCommand;
            _gameQuery = gameQuery;
            _gameClientList = new List<GameClient>();
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
        public void AddClient(GameClient gameClient)
        {
            _gameClientList.Add(gameClient);
        }
        public void CloseClient(GameClient gameClient)
        {
            gameClient.TcpClient.Close();
            _gameClientList.Remove(gameClient);
        }
        public async Task<bool> WriteAsync<T>(TcpClient tcpClient, T obj) where T : ResponseModelBase
        {
            return await _protocol.WriteAsync(tcpClient, obj);
        }
        public async Task<object> ReadAsync(TcpClient tcpClient)
        {
            return await _protocol.ReadAsync(tcpClient);
        }
        
        public void PushGameQueues(ResponseModelBase response, Func<GameClient,bool> expression)
        {
            _gameClientList.Where(expression).ToList().ForEach(x => x.GameQueue.Enqueue(response));
        }
        public void CancelFormerConnection(User user)
        {
            var gameClient = _gameClientList.FirstOrDefault(x => x.User.Id == user.Id);
            gameClient?.CancellationTokenSource.Cancel();
            while (gameClient != null && gameClient.TcpClient.Connected) { }
        }

        public async Task<bool> OpenNewConnectionAsync(TcpClient tcpClient)
        {
            _logger.LogInformation(EventId.GameServer,"Handshake starting.");
            if (!await _protocol.HandShakeAsync(tcpClient))
            {
                _logger.LogWarning(EventId.GameServer,"Handshake failed.");
                return false;
            }
            _logger.LogInformation(EventId.GameServer, "Handshake successful.");
            
            return true;
        }

    }
}