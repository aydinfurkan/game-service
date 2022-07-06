using GameService;
using GameService.Anticorruption.Extensions;
using GameService.Application.Commands;
using GameService.Controllers;
using GameService.Infrastructure.Extensions;
using GameService.TcpServer.Abstractions;
using GameService.TcpServer.Extensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;

IHostBuilder builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostContext, services) =>
    {
        services
            .AddSingleton<ICharacterController, CharacterController>()
            .AddMediatR(typeof(ClientCommand)) // This is for adding handlers 
            .AddInfrastructure()
            .AddAnticorruption(hostContext.Configuration)
            .AddTcpServer()
            .AddHostedService<Worker>();
    })
    .ConfigureLogging(configLogging =>
    {
        //configLogging.ClearProviders();
        configLogging.AddNLog();
        
    });

IHost app = builder.Build();

await app.RunAsync();