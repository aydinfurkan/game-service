using GameService.Application.Commands;
using GameService.Contract.ReceiveModels;
using GameService.Contract.ResponseModels;
using GameService.TcpServer.Controllers;
using MediatR;

namespace GameService.Application.Handlers;

public class SkillStateModelHandler: AsyncRequestHandler<ClientInputCommand<ChangeSkillStateCommand>>
{
    private readonly Server _server;
    
    public SkillStateModelHandler(
        Server server)
    {
        _server = server;
    }
    
    protected override Task Handle(ClientInputCommand<ChangeSkillStateCommand> command, CancellationToken cancellationToken)
    {
        var ok = command.Client.Character.TryCastSkill(command.Input.SkillState, out var castSkill);
        if (!ok) return Task.CompletedTask;
            
        var responseSkillStateModel = new Contract.ResponseModels.SkillStateModel
        {
            CharacterId = command.Client.Character.Id,
            TargetCharacterId = command.Client.Character.Target.Id,
            SkillState = command.Input.SkillState
        };
        _server.PushGameQueues(responseSkillStateModel, x => x.Character.Id != command.Client.Character.Id);

        if (castSkill.HealthChange(out var result))
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