using System.Net;
using System.Net.Sockets;
using GameService.TcpServer.Infrastructure.Protocol;
using Microsoft.Extensions.DependencyInjection;

namespace GameService.TcpServer.Infrastructure.Extensions;

public static class TcpServerInfrastructureServiceCollectionExtension
{
    public static IServiceCollection AddTcpServerInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton(new TcpListener(IPAddress.Any, 5000))
            .AddSingleton<IProtocol, GameProtocol>();
        return services;
    }
}