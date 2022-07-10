using System.Timers;
using GameService.Contract.ReceiveModels;
using GameService.Domain.Entities;
using GameService.TcpServer.Controllers;

namespace GameService.TcpServer.Abstractions;

public interface ICharacterController
{
    Task Send<T>(Game game, Client client, T receivedData) where T : CommandBaseData;

    Task Tick(Game game, Client client, ElapsedEventArgs elapsedEventArgs);

    Task Disconnect(Game game, Client client);
}