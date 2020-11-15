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
            BaseServerModell baseServer = new BaseServerModell(IPAddress.Any, 55310);
            int maxClient = 3;
          
            baseServer.Start();
            Task.Run(() =>
            {
                baseServer.Listen(maxClient);
            });
          
            var task = Task.Run(() =>
            {
                Task.Delay(500).Wait();
            });
            task.Wait();
          
            Assert.That(baseServer.taskList.Count == maxClient);
            baseServer.Stop();
        }
    }
}