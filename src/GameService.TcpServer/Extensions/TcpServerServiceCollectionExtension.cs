using GameService.TcpServer.Controllers;
using GameService.TcpServer.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace GameService.TcpServer.Extensions;

public static class TcpServerServiceCollectionExtension
{
    public static IServiceCollection AddTcpServer(this IServiceCollection services)
    {
        services.AddTcpServerInfrastructure()
            .AddSingleton<Server>()
            .AddSingleton<ServerController>();
        return services;
    }
}