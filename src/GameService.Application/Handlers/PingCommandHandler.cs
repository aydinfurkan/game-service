using GameService.Application.Commands;
using GameService.Contract.Commands;
using GameService.Contract.ResponseModels;
using GameService.TcpServer.Entities;
using MediatR;

namespace GameService.Application.Handlers;

public class PingCommandHandler: AsyncRequestHandler<ClientInputCommand<PingCommand>>
{
    private readonly Server _server;
    
    public PingCommandHandler(
        Server server)
    {
        _server = server;
    }
    
    protected override Task Handle(ClientInputCommand<PingCommand> command, CancellationToken cancellationToken)
    {
        if (command.Client.Character == null)
        {
            return Task.CompletedTask;
        }
        
        var responseCharacterHealth = new PongModel()
        {
            PingSentTime = command.Input.SentTime
        };
        _server.PushGameQueues(responseCharacterHealth, x => x.Value.Character?.Id == command.Client.Character.Id);
        

        return Task.CompletedTask;
    }
}