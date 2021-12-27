using System;
using System.Net.Sockets;
using GameService.Domain.Entity;

namespace GameService.Controller
{
    public class GameClient
    {
        public TcpClient TcpClient { get; }
        public User User { get; }
        public Character Character { get; }

        public GameClient(TcpClient tcpClient, User user, Character character)
        {
            TcpClient = tcpClient;
            User = user;
            Character = character;
        }
        
        
    }
}