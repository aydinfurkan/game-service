using System.Timers;
using GameService.Contract.Commands;
using GameService.Domain.Entities;
using GameService.TcpServer.Controllers;

namespace GameService.TcpServer.Abstractions;

public interface ICharacterController
{
    Task SendAsync<T>(Game game, Client client, T receivedData) where T : CommandBaseData;

    Task TickAsync(Game game, Client client, ElapsedEventArgs elapsedEventArgs);

    Task DisconnectAsync(Game game, Client client);
}