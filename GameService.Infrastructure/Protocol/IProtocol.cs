using System.Net.Sockets;
using System.Threading.Tasks;
using GameService.Infrastructure.Protocol.ResponseModels;

namespace GameService.Infrastructure.Protocol
{
    public interface IProtocol
    {
        public bool Write<T>(TcpClient client, T obj) where T : ResponseModelBase;
        public object Read(TcpClient client);
        public void HandShake(TcpClient client);
    }
}