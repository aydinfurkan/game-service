using System;
using System.Threading;
using System.Threading.Tasks;
using GameService.Controller;
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
            try
            {
                await Task.Run(() => _serverController.Init(cancellationToken), cancellationToken);
            }
            catch(Exception exception)
            {
                _logger.LogError(exception.Message);
            }

            _logger.LogInformation("GameService Exited.");
        }
    }
}