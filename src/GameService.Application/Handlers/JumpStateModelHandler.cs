using GameService.Application.Commands;
using GameService.TcpServer.Controllers;
using MediatR;
using JumpStateModel = GameService.Contract.ReceiveModels.JumpStateModel;

namespace GameService.Application.Handlers;

public class JumpStateModelHandler: AsyncRequestHandler<ClientInputCommand<JumpStateModel>>
{
    private readonly Server _server;
    
    public JumpStateModelHandler(
        Server server)
    {
        _server = server;
    }
    
    protected override Task Handle(ClientInputCommand<JumpStateModel> command, CancellationToken cancellationToken)
    {
        command.Client.Character.JumpState = command.Input.JumpState;
        var responseModel = new Contract.ResponseModels.JumpStateModel(command.Client.Character.Id, command.Input.JumpState);
        _server.PushGameQueues(responseModel, x => x.Character.Id != command.Client.Character.Id);
        return Task.CompletedTask;
    }
}