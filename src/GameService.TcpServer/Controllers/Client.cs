using System.Net.Sockets;
using AsyncAwaitBestPractices;
using GameService.Contract.Commands;
using GameService.Contract.ResponseModels;
using GameService.Domain.Entities;
using GameService.Domain.Entities.CharacterAggregate;
using GameService.TcpServer.Abstractions;
using GameService.TcpServer.Infrastructure.Protocol;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GameService.TcpServer.Controllers;

public class Client
{
    public Guid Id { get; private set; }
    public TcpClient TcpClient { get; }
    public User? User { get; private set; }
    public Character? Character { get; private set; }
    public CancellationTokenSource CancellationTokenSource { get; }
    public bool IsActive { get; private set; } = true;
    
    private readonly IProtocol _protocol;
    
    private readonly ILogger<Client> _logger;

    public Client(TcpClient tcpClient, IServiceProvider serviceProvider)
    {
        Id = Guid.NewGuid();
        TcpClient = tcpClient;

        _logger = serviceProvider.GetRequiredService<ILogger<Client>>();
        _logger.BeginScope("ClientId: {Id}",Id);
        
        _protocol = new GameProtocol();
        CancellationTokenSource = new CancellationTokenSource();
        tcpClient.NoDelay = true;
    }

    public void SetUser(User user)
    {
        User = user;
    }

    public void SetCharacter(Character character)
    {
        Character = character;
    }

    public async Task WriteAsync<T>(T obj) where T : ResponseModelData
    {
        await _protocol.WriteAsync(TcpClient, obj);
    }
    
    public async Task<CommandBaseData?> ReadAsync()
    {
        return await _protocol.ReadAsync(TcpClient);
    }
    
    public async Task HandShakeAsync()
    {
        _logger.LogInformation("Handshake started");
        await _protocol.HandShakeAsync(TcpClient);
        _logger.LogInformation($"Handshake succeed");
    }

    public async Task SubscribeClientAsync(Game game, ICharacterController characterController)
    {
        _logger.LogInformation("Subscribe process start");
        while (true)
        {
            var input = await ReadAsync();
            if (input == null) continue;

            characterController.SendAsync(game, this, input).SafeFireAndForget();
        }
    }
    
    public void Close()
    {
        CancellationTokenSource.Cancel();
        TcpClient.Close();
        IsActive = false;
    }
}