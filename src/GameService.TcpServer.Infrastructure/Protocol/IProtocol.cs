using System.Net.Sockets;
using GameService.Contract.Commands;
using GameService.Contract.ResponseModels;

namespace GameService.TcpServer.Infrastructure.Protocol;

public interface IProtocol
{
    public Task WriteAsync<T>(TcpClient client, T obj) where T : ResponseModelData;
    public Task<CommandBaseData?> ReadAsync(TcpClient client);
    public Task HandShakeAsync(TcpClient client);
}