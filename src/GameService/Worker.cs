using System.Net.Sockets;
using AsyncAwaitBestPractices;
using GameService.Common.Logger;
using GameService.Domain.Entities;
using GameService.TcpServer.Abstractions;
using GameService.TcpServer.Entities;
using GameService.TcpServer.Infrastructure.Protocol;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GameService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ICharacterController _characterController;
    private readonly Server _server;
    private readonly Game _game;
    private readonly IServiceProvider _services;
    private readonly IProtocol _protocol;

    public Worker(
        ILogger<Worker> logger, 
        Server server, 
        ICharacterController characterController, 
        ILoggerFactory logFactory, 
        IServiceProvider services, 
        IProtocol protocol)
    {
        _logger = logger;
        _server = server;
        _characterController = characterController;
        _services = services;
        _protocol = protocol;
        _game = new Game();
        ApplicationLogging.LoggerFactory = logFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        this.LogThreadCountAsync(cancellationToken).SafeFireAndForget();
        await _server.InitAsync(OnClientAcceptedAsync, cancellationToken);
    }

    private async Task OnClientAcceptedAsync(TcpClient tcpClient)
    {
        using var scope = _services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Client>>();
        var client = new Client(tcpClient, _protocol, _characterController, logger);

        try
        {
            await client.HandShakeAsync();
            await client.StartAsync(_game);
        }
        catch (Exception e)
        {
            logger.LogError("{Exception}", e.Message);
        }
        finally
        {
            client.Close(_game);
            _server.RemoveClient(client);
        }
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