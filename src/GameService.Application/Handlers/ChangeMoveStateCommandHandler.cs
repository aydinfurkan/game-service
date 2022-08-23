using GameService.Application.Commands;
using GameService.Contract.Commands;
using GameService.TcpServer.Controllers;
using MediatR;

namespace GameService.Application.Handlers;

public class ChangeMoveStateCommandHandler: AsyncRequestHandler<ClientInputCommand<ChangeMoveStateCommand>>
{
    private readonly Server _server;
    
    public ChangeMoveStateCommandHandler(
        Server server)
    {
        _server = server;
    }
    
    protected override Task Handle(ClientInputCommand<ChangeMoveStateCommand> command, CancellationToken cancellationToken)
    {
        if (command.Client.Character == null)
        {
            return Task.CompletedTask;
        }
        
        command.Client.Character.ChangeMoveState(command.Input);
        
        var responseModel = new Contract.ResponseModels.MoveStateModel
        {
            CharacterId = command.Client.Character.Id,
            MoveState = command.Input.MoveState
        };
        
        _server.PushGameQueues(responseModel, x => x.Character?.Id != command.Client.Character.Id);
        
        return Task.CompletedTask;
    }
}