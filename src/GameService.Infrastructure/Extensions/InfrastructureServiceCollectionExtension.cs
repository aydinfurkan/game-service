using GameService.Infrastructure.SafeFireAndForget;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GameService.Infrastructure.Extensions;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IServiceProvider serviceProvider)
    {
        services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
        SafeFireAndForgetSettings.Initialize(serviceProvider.GetRequiredService<ILogger<SafeFireAndForgetSettings>>());
        return services;
    }
}