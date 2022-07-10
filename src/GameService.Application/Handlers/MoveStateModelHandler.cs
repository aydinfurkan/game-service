using GameService.Application.Commands;
using GameService.Contract.ReceiveModels;
using GameService.TcpServer.Controllers;
using MediatR;

namespace GameService.Application.Handlers;

public class MoveStateModelHandler: AsyncRequestHandler<ClientInputCommand<ChangeMoveStateCommand>>
{
    private readonly Server _server;
    
    public MoveStateModelHandler(
        Server server)
    {
        _server = server;
    }
    
    protected override Task Handle(ClientInputCommand<ChangeMoveStateCommand> command, CancellationToken cancellationToken)
    {
        command.Client.Character.MoveState = command.Input.MoveState;
        var responseModel = new Contract.ResponseModels.MoveStateModel
        {
            CharacterId = command.Client.Character.Id,
            MoveState = command.Input.MoveState
        };
        _server.PushGameQueues(responseModel, x => x.Character.Id != command.Client.Character.Id);
        return Task.CompletedTask;
    }
}