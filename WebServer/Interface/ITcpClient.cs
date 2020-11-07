using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace WebServer.Interface
{
    public interface ITcpClient : IDisposable
    {
        Stream GetStream();
        EndPoint RemoteEndPoint { get; }
        void Connect(string ip, int port);
        void Close();

        string ReadToEnd();
    }
}