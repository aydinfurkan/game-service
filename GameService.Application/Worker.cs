using System;
using System.Threading;
using System.Threading.Tasks;
using GameService.Controllers;
using GameService.Infrastructure.Logger;
using Microsoft.Extensions.Hosting;

namespace GameService
{
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
}