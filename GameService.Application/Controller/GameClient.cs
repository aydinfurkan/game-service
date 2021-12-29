using System.Net.Sockets;
using System.Threading;
using GameService.Domain.Entity;
using GameService.Infrastructure.Logger;

namespace GameService.Controller
{
    public class GameClient
    {
        public TcpClient TcpClient { get; }
        public User User { get; }
        public Character Character { get; }
        public CancellationTokenSource CancellationTokenSource { get; }
        public int CorrelationId { get; }

        public GameClient(TcpClient tcpClient, User user, Character character)
        {
            TcpClient = tcpClient;
            User = user;
            Character = character;
            CancellationTokenSource = new CancellationTokenSource();
            CorrelationId = EventId.New();
        }
        
        
    }
}