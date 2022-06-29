using GameService.Anticorruption;
using GameService.Application;
using GameService.Domain;
using GameService.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

IHostBuilder builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostContext, services) =>
    {
        services.AddDomainModule()
            .AddInfrastructureModule()
            .AddAnticorruptionModule(hostContext.Configuration)
            .AddApplicationModule()
            .AddHostedService<Worker>();
    })
    .ConfigureLogging(configLogging =>
    {
        //configLogging.ClearProviders();
        configLogging.AddNLog();
        
    });

IHost app = builder.Build();

await app.RunAsync();