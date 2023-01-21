using GameService.Anticorruption.UserService.Configs;
using GameService.Anticorruption.UserService.UserService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameService.Anticorruption.UserService.Extensions;

public static class AnticorruptionModule
{
    public static IServiceCollection AddAnticorruption(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<UserServiceSettings>(configuration.GetSection("AnticorruptionSettings:UserServiceSettings"));
        services.AddTransient<HttpClient>();
        services.AddTransient<IUserAntiCorruption, UserAntiCorruption>();
        return services;
    }
}