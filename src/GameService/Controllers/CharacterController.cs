using System.Timers;
using GameService.Application.Commands;
using GameService.Contract.Commands;
using GameService.Domain.Entities;
using GameService.TcpServer.Abstractions;
using GameService.TcpServer.Controllers;
using MediatR;

namespace GameService.Controllers;

public class CharacterController: ICharacterController
{
    private readonly IMediator _mediator;

    public CharacterController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Send<T>(Game game, Client client, T receivedData) where T : CommandBaseData
    {
        switch (receivedData)
        {
            case VerificationCommand m:
            {
                var command = new ClientInputCommand<VerificationCommand>(game, client, m);
                await _mediator.Send(command);
                return;
            }
            case ChangePositionCommand m:
            {
                var command = new ClientInputCommand<ChangePositionCommand>(game, client, m);
                await _mediator.Send(command);
                return;
            }
            case ChangeQuaternionCommand m:
            {
                var command = new ClientInputCommand<ChangeQuaternionCommand>(game, client, m);
                await _mediator.Send(command);
                return;
            }
            case ChangeMoveStateCommand m:
            {
                var command = new ClientInputCommand<ChangeMoveStateCommand>(game, client, m);
                await _mediator.Send(command);
                return;
            }
            case ChangeJumpStateCommand m:
            {
                var command = new ClientInputCommand<ChangeJumpStateCommand>(game, client, m);
                await _mediator.Send(command);
                return;
            }
            case CastSkillCommand m:
            {
                var command = new ClientInputCommand<CastSkillCommand>(game, client, m);
                await _mediator.Send(command);
                return;
            }
            case SelectCharacterCommand m:
            {
                var command = new ClientInputCommand<SelectCharacterCommand>(game, client, m);
                await _mediator.Send(command);
                return;
            }
        }
    }
    public async Task Tick(Game game, Client client, ElapsedEventArgs elapsedEventArgs)
    {
        var command = new ClientInputCommand<ElapsedEventArgs>(game, client, elapsedEventArgs);
        await _mediator.Send(command);
    }
    
    public async Task Disconnect(Game game, Client client)
    {
        var command = new ClientCommand(game, client);
        await _mediator.Send(command);
    }
}