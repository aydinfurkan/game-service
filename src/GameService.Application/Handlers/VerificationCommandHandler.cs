using GameService.Anticorruption.UserService.UserService;
using GameService.Application.Commands;
using GameService.Contract.Commands;
using GameService.Contract.ResponseModels;
using GameService.TcpServer.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GameService.Application.Handlers;

public class VerificationCommandHandler: AsyncRequestHandler<ClientInputCommand<VerificationCommand>>
{
    private readonly Server _server;
    private readonly IUserAntiCorruption _userAntiCorruption;
    private readonly ILogger<VerificationCommandHandler> _logger;
    
    public VerificationCommandHandler(
        IUserAntiCorruption userAntiCorruption, 
        Server server, 
        ILogger<VerificationCommandHandler> logger)
    {
        _userAntiCorruption = userAntiCorruption;
        _server = server;
        _logger = logger;
    }
    
    protected override async Task Handle(ClientInputCommand<VerificationCommand> command, CancellationToken cancellationToken)
    {
        var input = command.Input;
        var client = command.Client;

        var userDto = await _userAntiCorruption.VerifyUserAsync(input.PToken);

        if (userDto == null)
        {
            command.Client.Close();
            return;
        }

        var user = userDto.ToModel();
        
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
        
        _server.PushGameQueues(userCharacterResponseModel, x => x.Value.Character?.Id == client.Character?.Id);

        var activeCharacters = command.Game.GetAllActiveCharacters().Select(x => x.ToCharacter()).ToList(); 
        var activeCharactersResponseModel = new ActiveCharacters
        {
            Characters = activeCharacters
        };
        _server.PushGameQueues(activeCharactersResponseModel, x => x.Value.Character?.Id == client.Character?.Id);

        var addCharacterResponseModel = new AddCharacter
        {
            Character = userCharacter
        };
        _server.PushGameQueues(addCharacterResponseModel, x => x.Value.Character?.Id != client.Character?.Id);
        
        command.Game.AddCharacter(client.Character);
    }
}