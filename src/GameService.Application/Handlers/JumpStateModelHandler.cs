using GameService.Application.Commands;
using GameService.Contract.ReceiveModels;
using GameService.TcpServer.Controllers;
using MediatR;

namespace GameService.Application.Handlers;

public class JumpStateModelHandler: AsyncRequestHandler<ClientInputCommand<ChangeJumpStateCommand>>
{
    private readonly Server _server;
    
    public JumpStateModelHandler(
        Server server)
    {
        _server = server;
    }
    
    protected override Task Handle(ClientInputCommand<ChangeJumpStateCommand> command, CancellationToken cancellationToken)
    {
        command.Client.Character.JumpState = command.Input.JumpState;
        var responseModel = new Contract.ResponseModels.JumpStateModel
        {
            CharacterId = command.Client.Character.Id,
            JumpState = command.Input.JumpState
        };
        _server.PushGameQueues(responseModel, x => x.Character.Id != command.Client.Character.Id);
        return Task.CompletedTask;
    }
}