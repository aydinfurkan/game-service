using System.Timers;
using GameService.Application.Commands;
using GameService.Contract.ResponseModels;
using GameService.TcpServer.Entities;
using MediatR;

namespace GameService.Application.Handlers;

public class TickHandler: AsyncRequestHandler<ClientInputCommand<ElapsedEventArgs>>
{
    private readonly Server _server;
    
    public TickHandler(Server server)
    {
        _server = server;
    }

    protected override Task Handle(ClientInputCommand<ElapsedEventArgs> command, CancellationToken cancellationToken)
    {
        if (command.Client.Character == null)
        {
            return Task.CompletedTask;
        }
        
        var ok = command.Client.Character.TryTick(command.Input.SignalTime, out var changeList);
        if (!ok) return Task.CompletedTask;

        foreach (var change in changeList)    
        {
            if (change.HealthChange(out var hResult))
            {
                var responseCharacterHealth = new CharacterHealth
                {
                    CharacterId = hResult.CharacterId,
                    Health = hResult.Health
                };
                _server.PushGameQueues(responseCharacterHealth);
            }

            if (change.ManaChange(out var mResult))
            {
                var responseCharacterMana = new CharacterMana
                {
                    CharacterId = mResult.CharacterId,
                    Mana = mResult.Mana
                };
                _server.PushGameQueues(responseCharacterMana);
            }
        }

        return Task.CompletedTask;
    }
}