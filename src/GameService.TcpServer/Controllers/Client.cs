using System.Collections.Concurrent;
using System.Net.Sockets;
using GameService.Contract.ResponseModels;
using GameService.Domain.Entities;
using GameService.Domain.Entities.CharacterAggregate;
using GameService.Infrastructure.Logger;

namespace GameService.TcpServer.Controllers;

public class Client
{
    public TcpClient TcpClient { get; }
    public User? User { get; private set; }
    public Character? Character { get; private set; }
    public BlockingCollection<ResponseModelData> GameQueue { get; }
    public CancellationTokenSource CancellationTokenSource { get; }
    public int CorrelationId { get; }

    public Client(TcpClient tcpClient)
    {
        TcpClient = tcpClient;
        CancellationTokenSource = new CancellationTokenSource();
        GameQueue = new BlockingCollection<ResponseModelData>();
        CorrelationId = EventId.New();
        tcpClient.NoDelay = true;
    }

    public void SetUser(User user)
    {
        User = user;
    }

    public void SetCharacter(Character character)
    {
        Character = character;
    }
}