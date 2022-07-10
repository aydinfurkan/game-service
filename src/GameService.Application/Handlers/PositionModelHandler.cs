using GameService.Application.Commands;
using GameService.Contract.ReceiveModels;
using GameService.TcpServer.Controllers;
using MediatR;

namespace GameService.Application.Handlers;

public class PositionModelHandler: AsyncRequestHandler<ClientInputCommand<ChangePositionCommand>>
{
    private readonly Server _server;
    
    public PositionModelHandler(
        Server server)
    {
        _server = server;
    }
    
    protected override Task Handle(ClientInputCommand<ChangePositionCommand> command, CancellationToken cancellationToken)
    {
        command.Client.Character.Position = command.Input.Position;
        var responseModel = new Contract.ResponseModels.PositionModel
        {
            CharacterId = command.Client.Character.Id,
            Position = command.Input.Position
        };
        _server.PushGameQueues(responseModel, x => x.Character.Id != command.Client.Character.Id);
        return Task.CompletedTask;
    }
}