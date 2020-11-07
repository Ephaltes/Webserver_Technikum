using System.Net.Sockets;

namespace WebServer.Interface
{
    public interface IServer
    {
        public void Start();
        public void Stop();
        public void Listen(int maxClient);
        public void HandleClient();

    }
}