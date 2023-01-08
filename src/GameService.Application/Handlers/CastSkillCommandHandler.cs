using GameService.Application.Commands;
using GameService.Contract.Commands;
using GameService.Contract.ResponseModels;
using GameService.TcpServer.Entities;
using MediatR;

namespace GameService.Application.Handlers;

public class CastSkillCommandHandler: AsyncRequestHandler<ClientInputCommand<CastSkillCommand>>
{
    private readonly Server _server;
    
    public CastSkillCommandHandler(
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
        
        var ok = command.Client.Character.TryCastSkill(command.Input, out var change);
        if (!ok) return Task.CompletedTask;
            
        if (change != null && change.ManaChange(out var result))
        {
            var responseCharacterHealth = new CharacterMana()
            {
                CharacterId = result.CharacterId,
                Mana = result.Mana
            };
            _server.PushGameQueues(responseCharacterHealth);
        }

        return Task.CompletedTask;
    }
}