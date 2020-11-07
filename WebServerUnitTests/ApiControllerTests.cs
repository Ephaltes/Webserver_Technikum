﻿using System;
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
                "POST /messages HTTP/1.1\r\n" +
                "Host: 127.0.0.1:6145\r\n" +
                "Connection: keep-alive\r\n" +
                "DNT: 1\r\n" +
                "Upgrade-Insecure-Requests: 1\r\n" +
                "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.111 Safari/537.36\r\n" +
                "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9\r\n" +
                "Sec-Fetch-Site: none\r\n" +
                "Sec-Fetch-Mode: navigate\r\n" +
                "Sec-Fetch-User: ?1\r\n" +
                "Sec-Fetch-Dest: document\r\n" +
                "Accept-Encoding: gzip, deflate, br\r\n" +
                "Accept-Language: en-US,en;q=0.9\r\n" +
                "sec-gpc: 1\r\n\r\n"+
                "Meine Nachricht";

            clientMock.Setup(_ => _.ReadToEnd()).Returns(header);

            var controller = new ApiController(clientMock.Object);

            string msg = controller.CreateResponse();
            
            Assert.That(!msg.Contains("ErrorMessage"));


        }
    }
}