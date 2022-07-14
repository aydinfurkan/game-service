using GameService.Application.Commands;
using GameService.Contract.Commands;
using GameService.Contract.ResponseModels;
using GameService.TcpServer.Controllers;
using MediatR;

namespace GameService.Application.Handlers;

public class ChangeSkillStateCommandHandler: AsyncRequestHandler<ClientInputCommand<CastSkillCommand>>
{
    private readonly Server _server;
    
    public ChangeSkillStateCommandHandler(
        Server server)
    {
        _server = server;
    }
    
    protected override Task Handle(ClientInputCommand<CastSkillCommand> command, CancellationToken cancellationToken)
    {
        if (command.Client.Character == null)
        {
            return Task.CompletedTask;
        }
        
        var ok = command.Client.Character.TryCastSkill(command.Input, out var castSkill);
        if (!ok) return Task.CompletedTask;
            
        var responseSkillStateModel = new SkillStateModel
        {
            CharacterId = command.Client.Character.Id,
            TargetCharacterId = command.Client.Character.Target.Id,
            SkillState = command.Input.SkillState
        };
        _server.PushGameQueues(responseSkillStateModel, x => x.Character?.Id != command.Client.Character.Id);

        if (castSkill != null && castSkill.HealthChange(out var result))
        {
            var responseCharacterHealth = new CharacterHealth
            {
                CharacterId = result.CharacterId,
                Health = result.Health
            };
            _server.PushGameQueues(responseCharacterHealth);
        }

        return Task.CompletedTask;
    }
}