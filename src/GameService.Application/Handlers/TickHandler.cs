using System.Timers;
using GameService.Application.Commands;
using GameService.Contract.ResponseModels;
using GameService.TcpServer.Controllers;
using MediatR;

namespace GameService.Application.Handlers;

public class TickHandler: AsyncRequestHandler<ClientInputCommand<ElapsedEventArgs>>
{
    private readonly Server _server;
    
    public TickHandler(
        Server server)
    {
        _server = server;
    }

    protected override Task Handle(ClientInputCommand<ElapsedEventArgs> command, CancellationToken cancellationToken)
    {
        var ok = command.Client.Character.Tick(command.Input.SignalTime, out var change);
        if (!ok) return Task.CompletedTask;

        if (change.HealthChange(out var hResult))
        {
            var responseCharacterHealth = new CharacterHealth(hResult.CharacterId, hResult.Health);
            _server.PushGameQueues(responseCharacterHealth);
        }

        if (change.ManaChange(out var mResult))
        {
            var responseCharacterMana = new CharacterMana(mResult.CharacterId, mResult.Mana);
            _server.PushGameQueues(responseCharacterMana);
        }

        return Task.CompletedTask;
    }
}