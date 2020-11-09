using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace WebServer.Interface
{
    /// <summary>
    /// TcpClient Interface for Unittesting
    /// </summary>
    public interface ITcpClient : IDisposable
    {
        Stream GetStream();
        EndPoint RemoteEndPoint { get; }
        void Connect(string ip, int port);
        void Close();

        string ReadToEnd();
    }
}