using GameService.Application.Commands;
using GameService.Contract.Commands;
using GameService.Contract.ResponseModels;
using GameService.TcpServer.Controllers;
using MediatR;

namespace GameService.Application.Handlers;

public class ExecuteSkillEffectCommandHandler: AsyncRequestHandler<ClientInputCommand<ExecuteSkillEffectCommand>>
{
    private readonly Server _server;
    
    public ExecuteSkillEffectCommandHandler(
        Server server)
    {
        _server = server;
    }
    
    protected override Task Handle(ClientInputCommand<ExecuteSkillEffectCommand> command, CancellationToken cancellationToken)
    {
        if (command.Client.Character == null)
        {
            return Task.CompletedTask;
        }
        
        var ok = command.Client.Character.TryExecuteSkill(command.Input, out var change);
        if (!ok) return Task.CompletedTask;
            
        if (change != null && change.HealthChange(out var result))
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