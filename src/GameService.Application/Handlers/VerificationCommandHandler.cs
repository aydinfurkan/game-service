using GameService.Anticorruption.UserService;
using GameService.Application.Commands;
using GameService.Contract.Commands;
using GameService.Contract.CommonModels;
using GameService.Contract.ResponseModels;
using GameService.TcpServer.Controllers;
using MediatR;

namespace GameService.Application.Handlers;

public class VerificationCommandHandler: AsyncRequestHandler<ClientInputCommand<VerificationCommand>>
{
    private readonly Server _server;
    private readonly IUserAntiCorruption _userAntiCorruption;
    
    public VerificationCommandHandler(
        IUserAntiCorruption userAntiCorruption, 
        Server server)
    {
        _userAntiCorruption = userAntiCorruption;
        _server = server;
    }
    
    protected override async Task Handle(ClientInputCommand<VerificationCommand> command, CancellationToken cancellationToken)
    {
        var input = command.Input;
        var client = command.Client;
        
        var user = (await _userAntiCorruption.VerifyUserAsync(input.PToken)).ToModel();
        _server.CancelFormerConnection(user);
        
        var character = user.CharacterList.FirstOrDefault(x => x.Id == input.CharacterId);

        if (character == null)
        {
            return; // TODO exception
        }
        
        client.SetUser(user);
        client.SetCharacter(character);
        
        if (client.Character == null)
        {
            return;
        }
        
        _server.AddClient(client);

        var userPlayer = character.ToUserCharacter();
        var userCharacter = character.ToCharacter();
        
        var userCharacterResponseModel = new ClientCharacter
        {
            UserCharacter = userPlayer
        };
        
        _server.PushGameQueues(userCharacterResponseModel, x => x.Character?.Id == client.Character?.Id);

        var activeCharacters = command.Game.GetAllActiveCharacters().Select(x => x.ToCharacter()).ToList(); 
        var activeCharactersResponseModel = new ActiveCharacters
        {
            Characters = activeCharacters
        };
        _server.PushGameQueues(activeCharactersResponseModel, x => x.Character?.Id == client.Character?.Id);

        var addCharacterResponseModel = new AddCharacter
        {
            Character = userCharacter
        };
        _server.PushGameQueues(addCharacterResponseModel, x => x.Character?.Id != client.Character?.Id);
        
        command.Game.AddCharacter(client.Character);
    }
}