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
    public class RequestContextTests
    {
        [Test]
        public void GetHttpVersionFromRequest()
        {
            string header =
                "GET /messages HTTP/1.1\r\nHost: 127.0.0.1:6145\r\nUser-Agent: curl/7.55.1\r\nAccept: */*\r\n\r\n";
            var requestContext = new RequestContext();
            requestContext.ParseRequestFromHeader(header);
            Assert.That(requestContext.HttpVersion == "HTTP/1.1");
        }

        [Test]
        public void ParseHttpHeaderFromRequestV1()
        {
            string header =
                "GET /messages HTTP/2.0\r\nHost: 127.0.0.1:6145\r\nConnection: keep-alive\r\nDNT: 1\r\nUpgrade-Insecure-Requests: 1\r\nUser-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.111 Safari/537.36\r\nAccept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9\r\nSec-Fetch-Site: none\r\nSec-Fetch-Mode: navigate\r\nSec-Fetch-User: ?1\r\nSec-Fetch-Dest: document\r\nAccept-Encoding: gzip, deflate, br\r\nAccept-Language: en-US,en;q=0.9\r\nsec-gpc: 1\r\n\r\n";
            var requestContext = new RequestContext();
            requestContext.ParseRequestFromHeader(header);
            Assert.That(requestContext.HttpHeader.Count == 13);
        }

        [Test]
        public void ParseHttpHeaderFromRequestV2()
        {
            string header =
                "GET /messages HTTP/1.1\r\nHost: 127.0.0.1:6145\r\nUser-Agent: curl/7.55.1\r\nAccept: */*\r\n\r\n";
            var requestContext = new RequestContext();
            requestContext.ParseRequestFromHeader(header);
            Assert.That(requestContext.HttpHeader.Count == 3);
        }

        [Test]
        public void GetHttpRequestFromRequestV1()
        {
            string header =
                "GET /messages HTTP/1.1\r\nHost: 127.0.0.1:6145\r\nUser-Agent: curl/7.55.1\r\nAccept: */*\r\n\r\n";
            var requestContext = new RequestContext();
            requestContext.ParseRequestFromHeader(header);
            Assert.That(requestContext.HttpRequest[0].ToLower() == "messages");
        }

        [Test]
        public void GetHttpRequestFromRequestV2()
        {
            string header = "GET / HTTP/1.1\r\nHost: 127.0.0.1:6145\r\nUser-Agent: curl/7.55.1\r\nAccept: */*\r\n\r\n";
            var requestContext = new RequestContext();
            requestContext.ParseRequestFromHeader(header);
            Assert.That(requestContext.HttpRequest.Count == 0);
        }

        [Test]
        public void GetHttpGetMethodFromRequest()
        {
            string header =
                "GET /messages HTTP/1.1\r\nHost: 127.0.0.1:6145\r\nUser-Agent: curl/7.55.1\r\nAccept: */*\r\n\r\n";
            var requestContext = new RequestContext();
            requestContext.ParseRequestFromHeader(header);
            Assert.That(requestContext.HttpMethod == HttpMethods.GET);
        }

        [Test]
        public void GetHttpPostMethodFromRequest()
        {
            string header =
                "POST /messages HTTP/1.1\r\nHost: 127.0.0.1:6145\r\nUser-Agent: curl/7.55.1\r\nAccept: */*\r\n\r\n";
            var requestContext = new RequestContext();
            requestContext.ParseRequestFromHeader(header);
            Assert.That(requestContext.HttpMethod == HttpMethods.POST);
        }

        [Test]
        public void GetHttpPutMethodFromRequest()
        {
            string header =
                "PUT /messages HTTP/1.1\r\nHost: 127.0.0.1:6145\r\nUser-Agent: curl/7.55.1\r\nAccept: */*\r\n\r\n";
            var requestContext = new RequestContext();
            requestContext.ParseRequestFromHeader(header);
            Assert.That(requestContext.HttpMethod == HttpMethods.PUT);
        }

        [Test]
        public void GetHttpDeleteMethodFromRequest()
        {
            string header =
                "DELETE /messages HTTP/1.1\r\nHost: 127.0.0.1:6145\r\nUser-Agent: curl/7.55.1\r\nAccept: */*\r\n\r\n";
            var requestContext = new RequestContext();
            requestContext.ParseRequestFromHeader(header);
            Assert.That(requestContext.HttpMethod == HttpMethods.DELETE);
        }

        [Test]
        public void GetHttpBodyFromRequestV1()
        {
            string header =
                "POST /messages HTTP/1.1\r\nHost: 127.0.0.1:6145\r\nUser-Agent: curl/7.55.1\r\nAccept: */*\r\nContent-Length: 15\r\nContent-Type: application/x-www-form-urlencoded\r\n\r\nHallo Wie gehts";
            var requestContext = new RequestContext();
            requestContext.ParseRequestFromHeader(header);
            Assert.That(requestContext.HttpBody == "Hallo Wie gehts");
        }

        [Test]
        public void GetHttpBodyFromRequestV2()
        {
            string header =
                "POST /messages HTTP/1.1\r\nContent-Type: text/plain\r\nUser-Agent: PostmanRuntime/7.26.5\r\nAccept: */*\r\nPostman-Token: 3a8b6112-f195-419b-81b0-e122522f224f\r\nHost: localhost:6145\r\nAccept-Encoding: gzip, deflate, br\r\nConnection: keep-alive\r\nContent-Length: 50\r\n\r\nhallo wie geht\r\nes dir meine liebe maus den heute?";
            var requestContext = new RequestContext();
            requestContext.ParseRequestFromHeader(header);
            Assert.That(requestContext.HttpBody == "hallo wie geht\r\nes dir meine liebe maus den heute?");
        }
    }
}