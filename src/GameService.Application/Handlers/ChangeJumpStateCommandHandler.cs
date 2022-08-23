using GameService.Application.Commands;
using GameService.Contract.Commands;
using GameService.TcpServer.Controllers;
using MediatR;

namespace GameService.Application.Handlers;

public class ChangeJumpStateCommandHandler: AsyncRequestHandler<ClientInputCommand<ChangeJumpStateCommand>>
{
    private readonly Server _server;
    
    public ChangeJumpStateCommandHandler(
        Server server)
    {
        _server = server;
    }
    
    protected override Task Handle(ClientInputCommand<ChangeJumpStateCommand> command, CancellationToken cancellationToken)
    {
        if (command.Client.Character == null)
        {
            return Task.CompletedTask;
        }
        
        command.Client.Character.ChangeJumpState(command.Input);
        
        var responseModel = new Contract.ResponseModels.JumpStateModel
        {
            CharacterId = command.Client.Character.Id,
            JumpState = command.Input.JumpState
        };
        
        _server.PushGameQueues(responseModel, x => x.Character?.Id != command.Client.Character.Id);
        
        return Task.CompletedTask;
    }
}