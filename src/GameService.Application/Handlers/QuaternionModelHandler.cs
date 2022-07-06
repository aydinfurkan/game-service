using GameService.Application.Commands;
using GameService.TcpServer.Controllers;
using MediatR;
using QuaternionModel = GameService.Contract.ReceiveModels.QuaternionModel;

namespace GameService.Application.Handlers;

public class QuaternionModelHandler: AsyncRequestHandler<ClientInputCommand<QuaternionModel>>
{
    private readonly Server _server;
    
    public QuaternionModelHandler(
        Server server)
    {
        _server = server;
    }
    
    protected override Task Handle(ClientInputCommand<QuaternionModel> command, CancellationToken cancellationToken)
    {
        command.Client.Character.Quaternion = command.Input.Quaternion;
        var responseModel = new Contract.ResponseModels.QuaternionModel(command.Client.Character.Id, command.Input.Quaternion);
        _server.PushGameQueues(responseModel, x => x.Character.Id != command.Client.Character.Id);
        return Task.CompletedTask;
    }
}