using GameService.Application.Commands;
using GameService.Contract.ResponseModels;
using GameService.TcpServer.Controllers;
using MediatR;
using SkillStateModel = GameService.Contract.ReceiveModels.SkillStateModel;

namespace GameService.Application.Handlers;

public class SkillStateModelHandler: AsyncRequestHandler<ClientInputCommand<SkillStateModel>>
{
    private readonly Server _server;
    
    public SkillStateModelHandler(
        Server server)
    {
        _server = server;
    }
    
    protected override Task Handle(ClientInputCommand<SkillStateModel> command, CancellationToken cancellationToken)
    {
        var ok = command.Client.Character.TryCastSkill(command.Input.SkillState, out var castSkill);
        if (!ok) return Task.CompletedTask;
            
        var responseSkillStateModel = new Contract.ResponseModels.SkillStateModel(command.Client.Character.Id, command.Client.Character.Target.Id, command.Input.SkillState);
        _server.PushGameQueues(responseSkillStateModel, x => x.Character.Id != command.Client.Character.Id);

        if (castSkill.HealthChange(out var result))
        {
            var responseCharacterHealth = new CharacterHealth(result.CharacterId, result.Health);
            _server.PushGameQueues(responseCharacterHealth);
        }

        return Task.CompletedTask;
    }
}