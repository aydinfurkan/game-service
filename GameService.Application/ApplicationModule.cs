using System.Net;
using System.Net.Sockets;
using GameService.Commands;
using GameService.Controllers;
using GameService.Handler;
using GameService.Infrastructure.Protocol;
using GameService.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace GameService
{
    public static class ApplicationModule
    {
        public static void AddApplicationModule(this IServiceCollection services)
        {
            services.AddSingleton<GameCommand>();
            services.AddSingleton<UserCommand>();
            services.AddSingleton<GameQuery>();
            services.AddSingleton(new TcpListener(IPAddress.Any, 5000));
            services.AddSingleton<IProtocol, GameProtocol>();
            services.AddSingleton<GameServer>();
            services.AddSingleton<InputHandler>();
            services.AddSingleton<InputQueue>();
            services.AddSingleton<ServerController>();
        }
    }
}