using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NUnit.Framework;
using WebServer;
using WebServer.API;
using WebServer.Model;

namespace WebServerUnitTests
{
    public class ResponseContextTests
    {
        [Test]
        public void BuildOnlyHttpHeader()
        {
            ResponseContext context = new ResponseContext();
            string ret = context.BuildResponse();
            Assert.That(ret == "HTTP/1.1 200\r\nServer: Server_404 Not Found\r\n");
        }
        [Test]
        public void BuildHttpHeaderWithPayload()
        {
            ResponseContext context = new ResponseContext();
            context.ResponseMessage.Add(new ResponseMessage(){Status = StatusCodes.OK,Id = 1,ErrorMessage = "Kein Error"});
            string ret = context.BuildResponse();
            Assert.That(ret.Length == 195);
        }
        
    }
}