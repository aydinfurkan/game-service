using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;
using GameService.Domain.Entities;
using GameService.Infrastructure.Logger;
using GameService.Infrastructure.Protocol.ResponseModels;

namespace GameService.Controllers
{
    public class GameClient
    {
        public TcpClient TcpClient { get; }
        public string PToken { get; }
        public User User { get; }
        public Character Character { get; }
        public ConcurrentQueue<ResponseModelBase> GameQueue { get; }
        public CancellationTokenSource CancellationTokenSource { get; }
        public int CorrelationId { get; }

        public GameClient(TcpClient tcpClient, string pToken, User user, Character character)
        {
            TcpClient = tcpClient;
            PToken = pToken;
            User = user;
            Character = character;
            CancellationTokenSource = new CancellationTokenSource();
            GameQueue = new ConcurrentQueue<ResponseModelBase>();
            CorrelationId = EventId.New();
            tcpClient.NoDelay = true;
        }
    }
}