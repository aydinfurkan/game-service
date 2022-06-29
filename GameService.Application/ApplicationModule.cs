using System.Net;
using System.Net.Sockets;
using GameService.Application.Commands;
using GameService.Application.Controllers;
using GameService.Application.Handler;
using GameService.Application.Queries;
using GameService.Infrastructure.Protocol;
using Microsoft.Extensions.DependencyInjection;

namespace GameService.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplicationModule(this IServiceCollection services)
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
        return services;
    }
}