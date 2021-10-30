using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace GameService.Controller
{
    public class TcpServer
    {
        private readonly ILogger _logger;
        private readonly TcpListener _listener;

        public TcpServer(ILogger logger, TcpListener tcpListener)
        {
            _logger = logger;
            _listener = tcpListener;
        }
        
        public void Start()
        {
            _listener.Start();
        }
        public void Stop()
        {
            _listener.Stop();
        }
        public TcpClient AcceptClient()
        {
            return _listener.AcceptTcpClient();
        }

        public bool Write(TcpClient client, object obj)
        {
            try
            {
                var stream = client.GetStream();
                var str = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                var bytes = Encoding.Default.GetBytes(str);

                stream.Write(bytes, 0, bytes.Length);

                _logger.LogInformation($"Thread : {Thread.CurrentThread.ManagedThreadId} --- Writing to client : {client.Client.RemoteEndPoint}");
                return true;
            }
            catch
            {
                _logger.LogInformation($"Thread : {Thread.CurrentThread.ManagedThreadId} --- Connection Error : {client.Client.RemoteEndPoint}");
                return false;
            }
        }
        
        public string Read(TcpClient client)
        {
            var stream = client.GetStream();

            _logger.LogInformation($"Thread : {Thread.CurrentThread.ManagedThreadId} --- Listening for client : {client.Client.RemoteEndPoint}");
            
            var data = new byte[client.ReceiveBufferSize];
            var bytes = stream.Read(data, 0, data.Length);
            var input = Encoding.ASCII.GetString(data, 0, bytes);
            
            _logger.LogInformation($"Thread : {Thread.CurrentThread.ManagedThreadId} --- Received input : {input}");

            return input;
        }
    }
}