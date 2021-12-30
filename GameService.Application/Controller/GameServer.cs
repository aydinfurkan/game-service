using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using GameService.Controller.RequestModel;
using GameService.Domain.Abstracts.AntiCorruption;
using GameService.Domain.Entity;
using GameService.Infrastructure.Logger;
using GameService.Protocol;

namespace GameService.Controller
{
    public class GameServer
    {
        private readonly IPLogger<GameServer> _logger;
        private readonly TcpListener _listener;
        private readonly IProtocol _protocol;
        private readonly List<GameClient> _gameClientList;
        private readonly IUserAntiCorruption _userAntiCorruption;

        public GameServer(IPLogger<GameServer> logger, TcpListener tcpListener, IProtocol protocol, IUserAntiCorruption userAntiCorruption)
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
        public async Task<TcpClient> AcceptClientAsync()
        {
            return await _listener.AcceptTcpClientAsync();
        }
        public void CloseClient(GameClient gameClient)
        {
            gameClient.TcpClient.Close();
            _gameClientList.Remove(gameClient);
        }
        public async Task<bool> WriteAsync(TcpClient tcpClient, object obj)
        {
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return await _protocol.WriteAsync(tcpClient, str);
        }
        public async Task<string> ReadAsync(TcpClient tcpClient)
        {
            return await _protocol.ReadAsync(tcpClient);
        }

        public async Task<GameClient> OpenNewConnectionAsync(TcpClient tcpClient)
        {
            _logger.LogInformation(EventId.GameServer,"Handshake starting.");
            if (!await _protocol.HandShakeAsync(tcpClient))
            {
                _logger.LogWarning(EventId.GameServer,"Handshake failed.");
                return null;
            }
            _logger.LogInformation(EventId.GameServer, "Handshake successful.");
            
            var gameClient = await VerifyClient(tcpClient);
            if (gameClient == null)
            {
                _logger.LogWarning(EventId.GameServer, "Verify client failed.");
                return null;
            }
            
            _gameClientList.Add(gameClient);
            return gameClient;
        }
        
        private async Task<GameClient> VerifyClient(TcpClient client)
        {
            var input = await ReadAsync(client);
            if(input == null) return null;
            
            var verifyUserRequestModel = Newtonsoft.Json.JsonConvert.DeserializeObject<VerifyUserRequestModel>(input);
            if (verifyUserRequestModel == null) return null;

            var (user, character) = await VerifyUser(verifyUserRequestModel);
            return new GameClient(client, user, character);
        }
        private async Task<(User, Character)> VerifyUser(VerifyUserRequestModel request)
        {
            var user = await _userAntiCorruption.VerifyUserAsync(request.Token);
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