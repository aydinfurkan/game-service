using System.Net.Sockets;
using System.Threading.Tasks;

namespace GameService.Protocol
{
    public interface IProtocol
    {
        public Task<bool> WriteAsync(TcpClient client, string str);
        public Task<string> ReadAsync(TcpClient client);
        public Task<bool> HandShakeAsync(TcpClient client);
    }
}