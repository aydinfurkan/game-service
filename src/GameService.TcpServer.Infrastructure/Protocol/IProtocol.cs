using System.Net.Sockets;
using GameService.Contract.ReceiveModels;
using GameService.Contract.ResponseModels;

namespace GameService.TcpServer.Infrastructure.Protocol;

public interface IProtocol
{
    public bool Write<T>(TcpClient client, T obj) where T : ResponseModelData;
    public CommandBaseData? Read(TcpClient client);
    public void HandShake(TcpClient client);
}