using GameService.TcpServer.Controllers;
using Microsoft.Extensions.Hosting;

namespace GameService;

public class Worker : BackgroundService
{
    private readonly ServerController _serverController;

    public Worker(ServerController serverController)
    {
        _serverController = serverController;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await Task.Run(() => _serverController.Init(cancellationToken), cancellationToken);
    }
}