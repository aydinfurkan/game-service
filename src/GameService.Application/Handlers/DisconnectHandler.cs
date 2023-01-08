using GameService.Anticorruption.UserService;
using GameService.Anticorruption.UserService.UserService;
using GameService.Application.Commands;
using GameService.Contract.ResponseModels;
using GameService.TcpServer.Entities;
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
        
        if (character == null)
        {
            return;
        }
        
        await _userAntiCorruption.ReplaceCharacterAsync(character);
        
        game.DeleteCharacter(character);
        
        var deleteCharacterResponseModel = new DeleteCharacter
        {
            CharacterId = character.Id
        };
        
        _server.PushGameQueues(deleteCharacterResponseModel, x => x.Value.Character?.Id != command.Client.Character?.Id);
    }
}