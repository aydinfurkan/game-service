using System.Net.Sockets;
using GameService.Contract.Commands;
using GameService.Contract.ResponseModels;

namespace GameService.TcpServer.Infrastructure.Protocol;

public interface IProtocol
{
    public Task WriteAsync<T>(TcpClient tcpClient, T obj) where T : ResponseModelData;
    public Task<CommandBaseData?> ReadAsync(TcpClient tcpClient, CancellationToken cancellationToken);
    public Task HandShakeAsync(TcpClient client);
}