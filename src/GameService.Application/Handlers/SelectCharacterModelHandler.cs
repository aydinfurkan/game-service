using GameService.Application.Commands;
using GameService.Contract.ReceiveModels;
using GameService.TcpServer.Controllers;
using MediatR;

namespace GameService.Application.Handlers;

public class SelectCharacterModelHandler: AsyncRequestHandler<ClientInputCommand<SelectCharacterModel>>
{
    private readonly Server _server;
    
    
    public SelectCharacterModelHandler(Server server)
    {
        _server = server;
    }
    
    protected override Task Handle(ClientInputCommand<SelectCharacterModel> command, CancellationToken cancellationToken)
    {
        command.Client.Character.Target = command.Game.GetAllActiveCharacters()
            .FirstOrDefault(x => x.Id == command.Input.CharacterId);
        return Task.CompletedTask;
    }
}