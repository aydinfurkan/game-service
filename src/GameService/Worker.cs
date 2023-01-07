using AsyncAwaitBestPractices;
using GameService.Domain.Entities;
using GameService.TcpServer.Abstractions;
using GameService.TcpServer.Controllers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GameService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ICharacterController _characterController;
    private readonly Server _server;
    private readonly Game _game;

    public Worker(ILogger<Worker> logger, Server server, ICharacterController characterController)
    {
        _logger = logger;
        _server = server;
        _characterController = characterController;
        _game = new Game();
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        this.LogThreadCountAsync(cancellationToken).SafeFireAndForget();
        await _server.InitAsync(OnClientAcceptedAsync, cancellationToken);
    }

    private async Task OnClientAcceptedAsync(Client client)
    {
        await client.HandShakeAsync();
        client.SubscribeClientAsync(_game, _characterController).SafeFireAndForget();
    }
    
    private async Task LogThreadCountAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation($"Working thread count : {System.Diagnostics.Process.GetCurrentProcess().Threads.Count}");
            // await Task.Delay(5*60*1000, cancellationToken);
            await Task.Delay(10*1000, cancellationToken);
        }
    }
}