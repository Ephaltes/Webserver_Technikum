using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using WebServer.Model;

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