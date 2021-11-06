using System.Net.Sockets;

namespace GameService.Protocol
{
    public interface IProtocol
    {
        public bool Write(TcpClient client, string str);
        public bool Read(TcpClient client, out string input);
        public bool HandShake(TcpClient client);
    }
}