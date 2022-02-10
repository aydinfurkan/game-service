using System.Net.Http;
using GameService.AntiCorruption.Configs;
using GameService.AntiCorruption.UserService;
using Microsoft.Extensions.DependencyInjection;

namespace GameService.AntiCorruption
{
    public static class AntiCorruptionModule
    {
        public static void AddAntiCorruptionModule(this IServiceCollection services)
        {
            services.AddTransient<HttpClient>();
            services.AddTransient<IUserAntiCorruption, UserAntiCorruption>();
        }
    }
}