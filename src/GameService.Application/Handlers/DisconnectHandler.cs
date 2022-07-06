using GameService.Anticorruption.UserService;
using GameService.Application.Commands;
using GameService.Contract.ResponseModels;
using GameService.TcpServer.Controllers;
using MediatR;

namespace GameService.Application.Handlers;

public class DisconnectHandler: AsyncRequestHandler<ClientCommand>
{
    private readonly Server _server;
    private readonly IUserAntiCorruption _userAntiCorruption;
    
    public DisconnectHandler(
        Server server,
        IUserAntiCorruption userAntiCorruption)
    {
        _server = server;
        _userAntiCorruption = userAntiCorruption;
    }
    
    protected override async Task Handle(ClientCommand command, CancellationToken cancellationToken)
    {
        var client = command.Client;
        var game = command.Game;
        var character = client.Character;
        
        await _userAntiCorruption.ReplaceCharacterAsync(character);
        
        game.DeleteCharacter(client.Character);
        
        var deleteCharacterResponseModel = new DeleteCharacter
        {
            CharacterId = command.Client.Character.Id
        };
        _server.PushGameQueues(deleteCharacterResponseModel, x => x.Character.Id != command.Client.Character.Id);
    }
}