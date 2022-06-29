using GameService.Domain.Components;
using GameService.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace GameService.Domain;

public static class DomainModule
{
    public static IServiceCollection AddDomainModule(this IServiceCollection services)
    {
        services.AddSingleton<ActiveCharacters>();
        services.AddSingleton<Game>();
        return services;
    }
}