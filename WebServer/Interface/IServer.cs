using System.Net.Sockets;

namespace WebServer.Interface
{
    /// <summary>
    /// Interface for ServerWrapper class
    /// that can start/stop server,
    /// listen to maxClient and handle the client
    /// </summary>
    public interface IServer
    {
        public void Start();
        public void Stop();
        public void Listen(int maxClient);
        public void HandleClient();

    }
}