using System.Timers;
using GameService.Application.Commands;
using GameService.Contract.ReceiveModels;
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

    public async Task Send<T>(Game game, Client client, T receivedData) where T : ReceiveModelData
    {
        switch (receivedData)
        {
            case VerificationModel m:
            {
                var command = new ClientInputCommand<VerificationModel>(game, client, m);
                await _mediator.Send(command);
                return;
            }
            case PositionModel m:
            {
                var command = new ClientInputCommand<PositionModel>(game, client, m);
                await _mediator.Send(command);
                return;
            }
            case QuaternionModel m:
            {
                var command = new ClientInputCommand<QuaternionModel>(game, client, m);
                await _mediator.Send(command);
                return;
            }
            case MoveStateModel m:
            {
                var command = new ClientInputCommand<MoveStateModel>(game, client, m);
                await _mediator.Send(command);
                return;
            }
            case JumpStateModel m:
            {
                var command = new ClientInputCommand<JumpStateModel>(game, client, m);
                await _mediator.Send(command);
                return;
            }
            case SkillStateModel m:
            {
                var command = new ClientInputCommand<SkillStateModel>(game, client, m);
                await _mediator.Send(command);
                return;
            }
            case SelectCharacterModel m:
            {
                var command = new ClientInputCommand<SelectCharacterModel>(game, client, m);
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