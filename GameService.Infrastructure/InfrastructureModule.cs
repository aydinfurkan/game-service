using GameService.Infrastructure.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GameService.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructureModule(this IServiceCollection services)
    {
        services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
        services.AddSingleton(typeof(IPLogger<>), typeof(ConsoleLogger<>));
        return services;
    }
}