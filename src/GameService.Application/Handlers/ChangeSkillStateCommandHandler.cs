using GameService.Application.Commands;
using GameService.Contract.Commands;
using GameService.Contract.ResponseModels;
using GameService.TcpServer.Controllers;
using MediatR;

namespace GameService.Application.Handlers;

public class ChangeSkillStateCommandHandler: AsyncRequestHandler<ClientInputCommand<ChangeSkillStateCommand>>
{
    private readonly Server _server;
    
    public ChangeSkillStateCommandHandler(
        Server server)
    {
        _server = server;
    }
    
    protected override Task Handle(ClientInputCommand<ChangeSkillStateCommand> command, CancellationToken cancellationToken)
    {
        if (command.Client.Character == null)
        {
            return Task.CompletedTask;
        }
        
        command.Client.Character.ChangeSkillState(command.Input);
        
        var responseModel = new SkillStateModel()
        {
            CharacterId = command.Client.Character.Id,
            TargetCharacterId = command.Client.Character.CurrentCastingTarget?.Id,
            SkillState = command.Input.SkillState
        };
        
        _server.PushGameQueues(responseModel, x => x.Character?.Id != command.Client.Character.Id);
        
        return Task.CompletedTask;
    }
}