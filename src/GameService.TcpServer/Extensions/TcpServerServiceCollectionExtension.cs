using GameService.TcpServer.Entities;
using GameService.TcpServer.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace GameService.TcpServer.Extensions;

public static class TcpServerServiceCollectionExtension
{
    public static IServiceCollection AddTcpServer(this IServiceCollection services)
    {
        services.AddTcpServerInfrastructure()
            .AddSingleton<Server>()
            .AddScoped<Client>();
        return services;
    }
}