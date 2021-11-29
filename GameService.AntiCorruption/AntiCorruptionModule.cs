using System.Net.Http;
using GameService.Domain.Abstracts.AntiCorruption;
using Microsoft.Extensions.DependencyInjection;

namespace GameService.AntiCorruption
{
    public static class AntiCorruptionModule
    {
        public static void AddAntiCorruptionModule(this IServiceCollection services)
        {
            services.AddTransient<IUserAntiCorruption, UserAntiCorruption>();
        }
    }
}