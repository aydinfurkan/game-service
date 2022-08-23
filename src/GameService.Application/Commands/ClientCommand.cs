using GameService.Domain.Entities;
using GameService.TcpServer.Controllers;
using MediatR;

namespace GameService.Application.Commands;

public class ClientCommand:  IRequest
{
    public readonly Game Game;
    public readonly Client Client;

    public ClientCommand(Game game, Client client)
    {
        Game = game;
        Client = client;
    }
}