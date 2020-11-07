using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using WebServer;
using WebServer.API;
using WebServer.Interface;

namespace WebServerUnitTests
{
    public class ApiControllerTests
    {
        [Test]
        public void GetMessages()
        {
            var clientMock = new Mock<ITcpClient>();

            string header =
                "GET /messages HTTP/1.1\n\r" +
                "Host: 127.0.0.1:6145\n\r" +
                "Connection: keep-alive\n\r" +
                "DNT: 1\n\r" +
                "Upgrade-Insecure-Requests: 1\n\r" +
                "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.111 Safari/537.36\n\r" +
                "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9\n\r" +
                "Sec-Fetch-Site: none\n\r" +
                "Sec-Fetch-Mode: navigate\n\r" +
                "Sec-Fetch-User: ?1\n\r" +
                "Sec-Fetch-Dest: document\n\r" +
                "Accept-Encoding: gzip, deflate, br\n\r" +
                "Accept-Language: en-US,en;q=0.9\n\r" +
                "sec-gpc: 1";
            
            clientMock.Setup(_ => _.ReadToEnd()).Returns(header);
            
            ApiController controller = new ApiController(clientMock.Object);
            var response = controller.CreateResponse();
        }
    }
}