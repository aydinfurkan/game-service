using GameService.Application.Commands;
using GameService.TcpServer.Controllers;
using MediatR;
using MoveStateModel = GameService.Contract.ReceiveModels.MoveStateModel;

namespace GameService.Application.Handlers;

public class MoveStateModelHandler: AsyncRequestHandler<ClientInputCommand<MoveStateModel>>
{
    private readonly Server _server;
    
    public MoveStateModelHandler(
        Server server)
    {
        _server = server;
    }
    
    protected override Task Handle(ClientInputCommand<MoveStateModel> command, CancellationToken cancellationToken)
    {
        command.Client.Character.MoveState = command.Input.MoveState;
        var responseModel = new Contract.ResponseModels.MoveStateModel(command.Client.Character.Id, command.Input.MoveState);
        _server.PushGameQueues(responseModel, x => x.Character.Id != command.Client.Character.Id);
        return Task.CompletedTask;
    }
}