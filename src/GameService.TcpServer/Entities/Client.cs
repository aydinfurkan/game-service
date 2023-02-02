using System.Net.Sockets;
using AsyncAwaitBestPractices;
using GameService.Contract.Commands;
using GameService.Contract.ResponseModels;
using GameService.Domain.Entities;
using GameService.Domain.Entities.CharacterAggregate;
using GameService.TcpServer.Abstractions;
using GameService.TcpServer.Infrastructure.Protocol;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GameService.TcpServer.Entities;

public class Client
{
    public Guid Id { get; }
    public TcpClient TcpClient { get; }
    public User? User { get; private set; }
    public Character? Character { get; private set; }
    public CancellationTokenSource CancellationTokenSource { get; }
    
    private readonly ILogger<Client> _logger;
    
    private readonly IProtocol _protocol;
    
    private readonly ICharacterController _characterController;
    
    private readonly List<System.Timers.Timer> _timers;

    public Client(TcpClient tcpClient, IProtocol protocol, ICharacterController characterController, ILogger<Client> logger)
    {
        Id = Guid.NewGuid();
        _timers = new List<System.Timers.Timer>();
        CancellationTokenSource = new CancellationTokenSource();

        _protocol = protocol;
        _characterController = characterController;
        TcpClient = tcpClient;
        _logger = logger;
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
        return await _protocol.ReadAsync(TcpClient, CancellationTokenSource.Token);
    }
    
    public async Task HandShakeAsync()
    {
        _logger.LogInformation("Handshake started");
        await _protocol.HandShakeAsync(TcpClient);
        _logger.LogInformation($"Handshake succeed");
    }

    public async Task StartAsync(Game game)
    {
        StartTick(game);
        // StartPing();
        await SubscribeAsync(game);
    }

    private async Task SubscribeAsync(Game game)
    {
        _logger.LogInformation("Subscribe process start");
        while (!CancellationTokenSource.IsCancellationRequested)
        {
            CancellationTokenSource.CancelAfter(10000);
            var input = await ReadAsync();
            if (input is VerificationCommand)
            {
                CancellationTokenSource.TryReset();
            }
            if (input == null)
            {
                return; // TODO client haber vermeden cikarsa diye konuldu. Bu durumu incelemek lazim
            }
            _logger.LogDebug("Message received. Input: {@Input}", JsonConvert.SerializeObject(input));
            _characterController.SendAsync(game, this, input).SafeFireAndForget();
        }
        _logger.LogInformation("Subscribe process end");
    }

    private void StartTick(Game game)
    {
        var tickTimer = new System.Timers.Timer(500) {Enabled = true};
        _timers.Add(tickTimer);
        tickTimer.Elapsed += (_, e) =>
        {
            _characterController.TickAsync(game, this, e).SafeFireAndForget();
        };
    }
    
    // private void StartPing()
    // {
    //     var pingTimer = new System.Timers.Timer(1000) {Enabled = true};
    //     _timers.Add(pingTimer);
    //     pingTimer.Elapsed += (_, e) =>
    //     {
    //         if (this.Character == null)
    //         {
    //             return;
    //         }
    //         var pingModel = new PongModel
    //         {
    //             PingSentTime = DateTime.UtcNow
    //         };
    //         WriteAsync(pingModel).SafeFireAndForget();
    //     };
    // }
    
    public void Close(Game game)
    {
        _logger.LogInformation("Client disconnected");
        CancellationTokenSource.Cancel();
        TcpClient.Close();
        _timers.ForEach(x =>
        {
            x.Stop();
            x.Dispose();
        });
        _characterController.DisconnectAsync(game, this);
    }
}