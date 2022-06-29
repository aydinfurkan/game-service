using System.Net.Http;
using GameService.Anticorruption.Configs;
using GameService.Anticorruption.UserService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameService.Anticorruption;

public static class AnticorruptionModule
{
    public static IServiceCollection AddAnticorruptionModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<UserServiceSettings>(configuration.GetSection("AnticorruptionSettings:UserServiceSettings"));
        services.AddTransient<HttpClient>();
        services.AddTransient<IUserAntiCorruption, UserAntiCorruption>();
        return services;
    }
}