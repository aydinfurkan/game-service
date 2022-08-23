using GameService.Application.Commands;
using GameService.Contract.Commands;
using GameService.TcpServer.Controllers;
using MediatR;

namespace GameService.Application.Handlers;

public class ChangePositionCommandHandler: AsyncRequestHandler<ClientInputCommand<ChangePositionCommand>>
{
    private readonly Server _server;
    
    public ChangePositionCommandHandler(
        Server server)
    {
        _server = server;
    }
    
    protected override Task Handle(ClientInputCommand<ChangePositionCommand> command, CancellationToken cancellationToken)
    {
        if (command.Client.Character == null)
        {
            return Task.CompletedTask;
        }
        
        command.Client.Character.ChangePosition(command.Input);
        
        var responseModel = new Contract.ResponseModels.PositionModel
        {
            CharacterId = command.Client.Character.Id,
            Position = command.Input.Position
        };
        
        _server.PushGameQueues(responseModel, x => x.Character?.Id != command.Client.Character.Id);
        
        return Task.CompletedTask;
    }
}