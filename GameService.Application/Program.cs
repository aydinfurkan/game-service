using GameService.AntiCorruption;
using GameService.Domain;
using GameService.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GameService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
            {
                //var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<Worker>();
                // services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                services.AddDomainModule();
                services.AddInfrastructureModule();
                services.AddAntiCorruptionModule();
                services.AddApplicationModule();
                services.AddHostedService<Worker>();
            });
    }
}