using GameService;
using GameService.Anticorruption.UserService.Extensions;
using GameService.Controllers;
using GameService.Infrastructure.Extensions;
using GameService.TcpServer.Abstractions;
using GameService.TcpServer.Extensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

IHostBuilder builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostContext, services) =>
    {
        var serviceProvider = services.BuildServiceProvider();
        
        services.AddSingleton<ICharacterController, CharacterController>()
            .AddMediatR(AppDomain.CurrentDomain.Load("GameService.Application")) // This is for adding handlers 
            .AddInfrastructure(serviceProvider)
            .AddAnticorruption(hostContext.Configuration)
            .AddTcpServer()
            .AddHostedService<Worker>();
    })
    .ConfigureLogging(configLogging =>
    {
        configLogging.ClearProviders();
        configLogging.AddConsole();
        configLogging.AddNLog();
        
        configLogging.AddSimpleConsole(options =>
        {
            options.IncludeScopes = true;
            options.SingleLine = true;
            options.TimestampFormat = "HH:mm:ss ";
        });
    });

IHost app = builder.Build();

await app.RunAsync();