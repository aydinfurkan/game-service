using GameService.Domain.Entities;
using GameService.TcpServer.Controllers;
using MediatR;

namespace GameService.Application.Commands;

public class ClientInputCommand<T>: IRequest where T: class
{
    public readonly Game Game;
    public readonly Client Client;
    public readonly T Input;

    public ClientInputCommand(Game game, Client client, T input)
    {
        Game = game;
        Client = client;
        Input = input;
    }
}