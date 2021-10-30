using GameService.Domain.Components;
using GameService.Domain.Entity;
using Microsoft.Extensions.DependencyInjection;

namespace GameService.Domain
{
    public static class DomainModule
    {
        public static void AddDomainModule(this IServiceCollection services)
        {
            services.AddSingleton<Game>();
        }
    }
}