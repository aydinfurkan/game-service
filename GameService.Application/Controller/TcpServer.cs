using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using GameService.Protocol;
using Microsoft.Extensions.Logging;

namespace GameService.Controller
{
    public class TcpServer
    {
        private readonly ILogger _logger;
        private readonly TcpListener _listener;
        private readonly IProtocol _protocol;

        public TcpServer(ILogger logger, TcpListener tcpListener, IProtocol protocol)
        {
            _logger = logger;
            _listener = tcpListener;
            _protocol = protocol;
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
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return _protocol.Write(client, str);
        }
        public bool Read(TcpClient client, out string input)
        {
            return _protocol.Read(client, out input);
        }
        public bool HandShake(TcpClient client)
        {
            return _protocol.HandShake(client);
        }
        
    }
}