using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using GameService.Controller.RequestModel;
using GameService.Domain.Abstracts.AntiCorruption;
using GameService.Domain.Entity;
using GameService.Protocol;
using Microsoft.Extensions.Logging;

namespace GameService.Controller
{
    public class GameServer
    {
        private readonly ILogger _logger;
        private readonly TcpListener _listener;
        private readonly IProtocol _protocol;
        private readonly List<GameClient> _gameClientList;
        private readonly IUserAntiCorruption _userAntiCorruption;

        public GameServer(ILogger logger, TcpListener tcpListener, IProtocol protocol, IUserAntiCorruption userAntiCorruption)
        {
            _logger = logger;
            _listener = tcpListener;
            _protocol = protocol;
            _userAntiCorruption = userAntiCorruption;
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
        public async Task<GameClient> AcceptClient()
        {
            var tcpClient = await _listener.AcceptTcpClientAsync();
            if (!_protocol.HandShake(tcpClient)) return null;
            
            var gameClient = await VerifyClient(tcpClient);
            if (gameClient == null) return null;
            
            _gameClientList.Add(gameClient);
            return gameClient;
        }
        public void CloseClient(GameClient gameClient)
        {
            gameClient.TcpClient.Close();
            _gameClientList.Remove(gameClient);
        }
        public bool Write(TcpClient tcpClient, object obj)
        {
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return _protocol.Write(tcpClient, str);
        }
        public bool Read(TcpClient tcpClient, out string input)
        {
            return _protocol.Read(tcpClient, out input);
        }
        
        private async Task<GameClient> VerifyClient(TcpClient client)
        {
            if(!Read(client, out var input)) return null;
            
            var verifyUserRequestModel = Newtonsoft.Json.JsonConvert.DeserializeObject<VerifyUserRequestModel>(input);
            if (verifyUserRequestModel == null) return null;

            var (user, character) = await VerifyUser(verifyUserRequestModel);
            return new GameClient(client, user, character);
        }
        private async Task<(User, Character)> VerifyUser(VerifyUserRequestModel request)
        {
            var user = await _userAntiCorruption.VerifyUser(request.Token);
            CancelFormerConnection(user);
            var character = user.CharacterList.FirstOrDefault(x =>
                x.CharacterId == request.CharacterId &&
                x.CharacterName == request.CharacterName &&
                x.CharacterClass == request.CharacterClass);
            return (user, character);
        }
        private void CancelFormerConnection(User user)
        {
            var gameClient = _gameClientList.FirstOrDefault(x => x.User.Id == user.Id);
            gameClient?.CancellationTokenSource.Cancel();
            while (gameClient != null && gameClient.TcpClient.Connected) { }
        }
        
    }
}