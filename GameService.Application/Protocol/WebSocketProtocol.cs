using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace GameService.Protocol
{
    public class WebSocketProtocol : IProtocol  // https://datatracker.ietf.org/doc/html/rfc6455#section-5.1
    {
        
        private readonly ILogger _logger;

        public WebSocketProtocol(ILogger logger)
        {
            _logger = logger;
        }
        
        public bool Write(TcpClient client, string str) // TODO make longer than 125
        {
            var stream = client.GetStream();
            var queue = new Queue<string>(SplitInGroups(str,125)); //Make it so the message is never longer than 125 (Split the message into parts & store them in a queue)
            var len = queue.Count;

            while (queue.Count > 0)
            {
                var header = GetHeader(queue.Count <= 1, queue.Count != len); //Get the header for a part of the queue

                var bytes = Encoding.UTF8.GetBytes(queue.Dequeue()); //Get part of the message out of the queue
                header = (header << 7) + bytes.Length; //Add the length of the part we are going to send

                //Send the header & message to client
                stream.Write(IntToByteArray((ushort)header));
                stream.Write(bytes);
            }

            return true;
        }
        
        public bool Read(TcpClient client, out string input)
        {
            var stream = client.GetStream();
            
            var bytes = new byte[client.ReceiveBufferSize];
            stream.Read(bytes, 0, bytes.Length);
            
            var fin = GetBit(bytes, 1);
            var mask = GetBit(bytes, 9);

            var opcode = GetInt(bytes[0], 5, 8);
            var msgLen = GetInt(bytes[1], 2,8);
            var offset = 2;

            switch (msgLen)
            {
                case 0:
                    input = string.Empty;
                    return true;
                case 126:
                    // was ToUInt16(bytes, offset) but the result is incorrect TODO
                    //msglen = BitConverter.ToUInt16(new [] { bytes[3], bytes[2] }, 0);
                    //offset = 4;
                    break;
                case 127:
                    // i don't really know the byte order, please edit this TODO
                    // msglen = BitConverter.ToUInt64(new byte[] { bytes[5], bytes[4], bytes[3], bytes[2], bytes[9], bytes[8], bytes[7], bytes[6] }, 0);
                    // offset = 10;
                    break;
            }

            if (!mask)
            {
                throw new Exception(); // TODO Exception
            }
            
            var decoded = new byte[msgLen];
            var masks = new [] { bytes[offset++], bytes[offset++], bytes[offset++], bytes[offset++] };

            for (var i = 0; i < msgLen; ++i)
                decoded[i] = (byte)(bytes[offset++] ^ masks[i % 4]);

            input = Encoding.UTF8.GetString(decoded);
            
            return true;
        }
        
        public bool HandShake(TcpClient client)
        {
            while(client.Available < 3){}
            
            _logger.LogInformation($"Thread : {Thread.CurrentThread.ManagedThreadId} --- HandShake process start with {client.Client.RemoteEndPoint}.");
            
            var data = new byte[client.ReceiveBufferSize];
            var bytes = client.GetStream().Read(data, 0, data.Length);
            var input = Encoding.ASCII.GetString(data, 0, bytes);

            if (!Regex.IsMatch(input, "^GET"))
            {
                _logger.LogInformation($"Thread : {Thread.CurrentThread.ManagedThreadId} --- HandShake process failed with {client.Client.RemoteEndPoint}.");
                return false;
            }
            
            const string eol = "\r\n"; // HTTP/1.1 defines the sequence CR LF as the end-of-line marker
                
            var swk = Regex.Match(input, "Sec-WebSocket-Key: (.*)").Groups[1].Value.Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
            var swkSha1Base64 = Convert.ToBase64String(System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(swk)));
            var response = Encoding.UTF8.GetBytes(
                "HTTP/1.1 101 Switching Protocols" + eol +
                "Connection: Upgrade" + eol +
                "Upgrade: websocket" + eol +
                "Sec-WebSocket-Accept: " + swkSha1Base64 + eol + eol);

            client.GetStream().Write(response, 0, response.Length);      
            
            _logger.LogInformation($"Thread : {Thread.CurrentThread.ManagedThreadId} --- HandShake process end successfully with {client.Client.RemoteEndPoint}.");
            return true;
        }
        
        private int GetHeader(bool finalFrame, bool contFrame)
        {
            var header = finalFrame ? 1 : 0;//fin: 0 = more frames, 1 = final frame
            header = (header << 1) + 0;//rsv1
            header = (header << 1) + 0;//rsv2
            header = (header << 1) + 0;//rsv3
            header = (header << 4) + (contFrame ? 0 : 1);//opcode : 0 = continuation frame, 1 = text
            header = (header << 1) + 0;//mask: server -> client = no mask

            return header;
        }
        
        private byte[] IntToByteArray(ushort value)
        {
            var ary = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(ary);
            }

            return ary;
        }

        public static IEnumerable<string> SplitInGroups(string original, int size)
        {
            var p = 0;
            var l = original.Length;
            while (l - p > size)
            {
                yield return original.Substring(p, size);
                p += size;
            }
            yield return original.Substring(p);
        }
        
        public static bool GetBit(byte[] bs, int bitNumber)
        {
            var b = bs[bitNumber / 8];
            var bN = bitNumber % 8;
            var s = 8 - bN;
            return (b & (1 << s)) != 0;
        }
        public static int GetInt(byte b, int startBit, int endBit)
        {
            var len = endBit - startBit + 1;
            var bn = 0;
            for (var i = 0; i < len; i++)
            {
                bn += (1 << i);
            }

            var s = 8 - endBit;
            return b & (bn << s);
        }
    }
}