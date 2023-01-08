using GameService.Application.Commands;
using GameService.Contract.Commands;
using GameService.TcpServer.Entities;
using MediatR;

namespace GameService.Application.Handlers;

public class ChangeQuaternionCommandHandler: AsyncRequestHandler<ClientInputCommand<ChangeQuaternionCommand>>
{
    private readonly Server _server;
    
    public ChangeQuaternionCommandHandler(
        Server server)
    {
        _server = server;
    }
    
    protected override Task Handle(ClientInputCommand<ChangeQuaternionCommand> command, CancellationToken cancellationToken)
    {
        if (command.Client.Character == null)
        {
            return Task.CompletedTask;
        }
        
        command.Client.Character.ChangeQuaternion(command.Input);
        
        var responseModel = new Contract.ResponseModels.QuaternionModel
        {
            CharacterId = command.Client.Character.Id,
            Quaternion = command.Input.Quaternion
        };
        
        _server.PushGameQueues(responseModel, x => x.Value.Character?.Id != command.Client.Character.Id);
        
        return Task.CompletedTask;
    }
}