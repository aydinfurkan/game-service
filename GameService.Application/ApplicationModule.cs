using System.Net;
using System.Net.Sockets;
using GameService.Commands;
using GameService.Controller;
using GameService.Protocol;
using GameService.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace GameService
{
    public static class ApplicationModule
    {
        public static void AddApplicationModule(this IServiceCollection services)
        {
            services.AddSingleton<GameCommand>();
            services.AddSingleton<GameQuery>();
            services.AddSingleton(new TcpListener(IPAddress.Any, 5000));
            services.AddSingleton<IProtocol, WebSocketProtocol>();
            services.AddSingleton<TcpServer>();
            services.AddSingleton<ServerController>();
        }
    }
}