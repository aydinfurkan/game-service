using GameService.AntiCorruption;
using GameService.AntiCorruption.Configs;
using GameService.Domain;
using GameService.Infrastructure;
using Microsoft.Extensions.Configuration;
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
                var env = hostContext.HostingEnvironment;
                
                var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
                    .AddUserSecrets<Program>()
                    .AddEnvironmentVariables();
                
                var configuration = builder.Build();

                services.Configure<UserServiceSettings>(configuration.GetSection("AntiCorruptionSettings:UserServiceSettings"));

                services.AddDomainModule();
                services.AddInfrastructureModule();
                services.AddAntiCorruptionModule();
                services.AddApplicationModule();
                services.AddHostedService<Worker>();
            });
    }
}