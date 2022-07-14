using GameService.Application.Commands;
using GameService.Contract.Commands;
using GameService.TcpServer.Controllers;
using MediatR;

namespace GameService.Application.Handlers;

public class SelectCharacterCommandHandler: AsyncRequestHandler<ClientInputCommand<SelectCharacterCommand>>
{
    private readonly Server _server;
    
    
    public SelectCharacterCommandHandler(Server server)
    {
        _server = server;
    }
    
    protected override Task Handle(ClientInputCommand<SelectCharacterCommand> command, CancellationToken cancellationToken)
    {
        command.Client.Character?.SelectCharacter(command.Input, command.Game);
        
        return Task.CompletedTask;
    }
}