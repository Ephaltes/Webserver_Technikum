using System.Net;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using WebServer;
using WebServer.API;
using WebServer.Interface;
using WebServer.Model;
using WebServer.RessourcenHandler;

namespace WebServerUnitTests
{
    public class ServerModellTests
    {
        [Test]
        public void MaxListenClients()
        {
            ServerModell server = new ServerModell(IPAddress.Any, 55310);
            int maxClient = 3;
          
            server.Start();
            Task.Run(() =>
            {
                server.Listen(maxClient);
            });
          
            var task = Task.Run(() =>
            {
                Task.Delay(500).Wait();
            });
            task.Wait();
          
            Assert.That(server.taskList.Count == maxClient);
            server.Stop();
        }
    }
}