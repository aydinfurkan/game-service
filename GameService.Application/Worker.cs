using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using GameService.Commands;
using GameService.Controller;
using GameService.Queries;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GameService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ServerController _serverController;

        public Worker(ILogger<Worker> logger, ServerController serverController)
        {
            _logger = logger;
            _serverController = serverController;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("GameService Executed.");
            await Task.Run(() => _serverController.Init(cancellationToken), cancellationToken);
            _logger.LogInformation("GameService Exited.");
        }
    }
}