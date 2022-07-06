using GameService.Anticorruption.UserService;
using GameService.Application.Commands;
using GameService.Contract.CommonModels;
using GameService.Contract.ReceiveModels;
using GameService.Contract.ResponseModels;
using GameService.TcpServer.Controllers;
using MediatR;

namespace GameService.Application.Handlers;

public class VerificationModelHandler: AsyncRequestHandler<ClientInputCommand<VerificationModel>>
{
    private readonly Server _server;
    private readonly IUserAntiCorruption _userAntiCorruption;
    
    public VerificationModelHandler(
        IUserAntiCorruption userAntiCorruption, 
        Server server)
    {
        _userAntiCorruption = userAntiCorruption;
        _server = server;
    }
    
    protected override async Task Handle(ClientInputCommand<VerificationModel> command, CancellationToken cancellationToken)
    {
        var input = command.Input;
        var client = command.Client;
        
        var user = (await _userAntiCorruption.VerifyUserAsync(input.PToken)).ToModel();
        _server.CancelFormerConnection(user);
        
        var character = user.CharacterList.FirstOrDefault(x => x.Id == input.CharacterId);

        client.SetUser(user);
        client.SetCharacter(character);
        
        _server.AddClient(client);

        var userPlayer = new UserCharacterDto(character);
        var userCharacter = new CharacterDto(character);
        var userCharacterResponseModel = new ClientCharacter(userPlayer);
        _server.PushGameQueues(userCharacterResponseModel, x => x.Character.Id == client.Character.Id);

        var activeCharacters = command.Game.GetAllActiveCharacters().Select(x => new CharacterDto(x)).ToList(); 
        var activeCharactersResponseModel = new ActiveCharacters(activeCharacters);
        _server.PushGameQueues(activeCharactersResponseModel, x => x.Character.Id == client.Character.Id);

        var addCharacterResponseModel = new AddCharacter(userCharacter);
        _server.PushGameQueues(addCharacterResponseModel, x => x.Character.Id != client.Character.Id);
        
        command.Game.AddCharacter(client.Character);
    }
}