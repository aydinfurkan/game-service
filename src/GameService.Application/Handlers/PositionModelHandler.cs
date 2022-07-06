using GameService.Application.Commands;
using GameService.TcpServer.Controllers;
using MediatR;
using PositionModel = GameService.Contract.ReceiveModels.PositionModel;

namespace GameService.Application.Handlers;

public class PositionModelHandler: AsyncRequestHandler<ClientInputCommand<PositionModel>>
{
    private readonly Server _server;
    
    public PositionModelHandler(
        Server server)
    {
        _server = server;
    }
    
    protected override Task Handle(ClientInputCommand<PositionModel> command, CancellationToken cancellationToken)
    {
        command.Client.Character.Position = command.Input.Position;
        var responseModel = Contract.ResponseModels.PositionModel.CreateInstance(command.Client.Character.Id, command.Input.Position);
        _server.PushGameQueues(responseModel, x => x.Character.Id != command.Client.Character.Id);
        return Task.CompletedTask;
    }
}