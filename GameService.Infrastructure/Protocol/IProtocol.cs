using System.Net.Sockets;
using System.Threading.Tasks;
using GameService.Infrastructure.Protocol.ResponseModels;

namespace GameService.Infrastructure.Protocol
{
    public interface IProtocol
    {
        public Task<bool> WriteAsync<T>(TcpClient client, T obj) where T : ResponseModelBase;
        public Task<object> ReadAsync(TcpClient client);
        public Task<bool> HandShakeAsync(TcpClient client);
    }
}