using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using WebServer.Interface;

namespace WebServer.Model
{
    public class TcpClient : ITcpClient
    {
        private System.Net.Sockets.TcpClient _client;
        public EndPoint RemoteEndPoint => _client.Client.RemoteEndPoint;
        public Stream GetStream() => _client.GetStream();

        public TcpClient(System.Net.Sockets.TcpClient client)
        {
            _client = client;
        }
        
        public void Dispose()
        {
            _client.Dispose();
        }
        
        public void Connect(string ip, int port)
        {
            _client.Connect(IPAddress.Parse(ip),port);
        }

        public void Close()
        {
            _client.Close();
        }

        public string ReadToEnd()
        {
            var stream = _client.GetStream();
            string data = "";
            do
            {
                Byte[] bytes = new Byte[4096];
                int i = stream.Read(bytes, 0, bytes.Length);
                data += System.Text.Encoding.ASCII.GetString(bytes, 0, i);
            } while (stream.DataAvailable);

            return data;
        }
    }
}