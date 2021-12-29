using System;
using System.Threading;
using System.Threading.Tasks;
using GameService.Controller;
using GameService.Infrastructure.Logger;
using Microsoft.Extensions.Hosting;

namespace GameService
{
    public class Worker : BackgroundService
    {
        private readonly IPLogger<Worker> _logger;
        private readonly ServerController _serverController;

        public Worker(IPLogger<Worker> logger, ServerController serverController)
        {
            _logger = logger;
            _serverController = serverController;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation(EventId.GameService, "GameService Executing.");
                await Task.Run(() => _serverController.Init(cancellationToken), cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(EventId.GameService,"ServerController Init exception.", exception);
            }
            finally
            {
                _logger.LogInformation(EventId.GameService, "GameService Executed.");
            }
        }
    }
}