using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using WebServer;
using WebServer.API;

namespace WebServerUnitTests
{
    public class ApiControllerTests
    {
        private TcpListener server = null;
        private TcpClient server_client = new TcpClient();
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            server = new TcpListener(IPAddress.Parse("127.0.0.1"),12345);
            server.Start();
        }

        [SetUp]
        public void SetUp()
        {
            server_client = new TcpClient();
            Task.Run(() => HandleClient(server));
        }

        [TearDown]
        public void TearDown()
        {
            server_client.Close();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            server.Stop();
        }
        private void HandleClient(TcpListener server)
        {
            server_client = server.AcceptTcpClient();
        }
        
        [Test]
        public void GetMessages()
        {
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse("127.0.0.1"),12345);
            client.GetStream().Write(Encoding.ASCII.GetBytes("GET /messages HTTP/1.1\r\nHost: 127.0.0.1:6145\r\nUser-Agent: curl/7.55.1\r\nAccept: */*\r\n\r\n"));

            while (!server_client.Connected)
            {
                Task.Delay(50);
            } 
            
            ApiController controller = new ApiController(server_client);
            string response = controller.CreateResponse();
            
            Assert.That(response.Length == 188);
            
            /*
                HTTP/1.1 200
                Server: Server_404 Not Found
                Content-Type: application/json; charset=UTF-8
                Content-Length: 75

                [
                  {
                    "ErrorMessage": "Message not Found",
                    "Status": 404
                  }
                ]
             */
            
        }
    }
}