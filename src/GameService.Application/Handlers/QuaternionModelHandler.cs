using GameService.Application.Commands;
using GameService.Contract.ReceiveModels;
using GameService.TcpServer.Controllers;
using MediatR;

namespace GameService.Application.Handlers;

public class QuaternionModelHandler: AsyncRequestHandler<ClientInputCommand<ChangeQuaternionCommand>>
{
    private readonly Server _server;
    
    public QuaternionModelHandler(
        Server server)
    {
        _server = server;
    }
    
    protected override Task Handle(ClientInputCommand<ChangeQuaternionCommand> command, CancellationToken cancellationToken)
    {
        command.Client.Character.Quaternion = command.Input.Quaternion;
        var responseModel = new Contract.ResponseModels.QuaternionModel
        {
            CharacterId = command.Client.Character.Id,
            Quaternion = command.Input.Quaternion
        };
        _server.PushGameQueues(responseModel, x => x.Character.Id != command.Client.Character.Id);
        return Task.CompletedTask;
    }
}