using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using GameService.Commands;
using GameService.Domain.Entities;
using GameService.Infrastructure.Logger;
using GameService.Infrastructure.Protocol;
using GameService.Infrastructure.Protocol.ResponseModels;
using GameService.Queries;

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
            _gameClientList.Remove(gameClient);
            gameClient.GameQueue.CompleteAdding();
            gameClient.CancellationTokenSource.Cancel();
            gameClient.TcpClient.Close();
        }
        public bool Write<T>(TcpClient tcpClient, T obj) where T : ResponseModelBase
        {
            return _protocol.Write(tcpClient, obj);
        }
        public object Read(TcpClient tcpClient)
        {
            return _protocol.Read(tcpClient);
        }
        
        public void PushGameQueues(ResponseModelBase response, Func<GameClient,bool> expression)
        {
            _gameClientList.Where(expression).ToList().ForEach(x => x.GameQueue.Add(response));
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
}