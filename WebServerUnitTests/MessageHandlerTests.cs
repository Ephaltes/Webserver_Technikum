using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using WebServer;
using WebServer.API;
using WebServer.Interface;
using WebServer.Model;
using WebServer.RessourceHandler;

namespace WebServerUnitTests
{
    public class MessageHandlerTests
    {
        [Test]
        public void GetMessages()
        {
            
            Mock<IRequestContext> context = new Mock<IRequestContext>();
            context.SetupGet(x => x.HttpBody).Returns("Meine Nachricht");
            context.SetupGet(x => x.HttpRequest).Returns(new List<string>() {"messages"});
            context.SetupGet(x => x.HttpMethod).Returns(HttpMethods.GET);
            
            IMessage messageModel = new MessageModel();
            MessageHandler handler = new MessageHandler(context.Object,messageModel);

            var response = handler.Handle();
            
            Assert.That(response.StatusCode == StatusCodes.OK);
        }
        
        [Test]
        public void PostMessage()
        {
            Mock<IRequestContext> context = new Mock<IRequestContext>();
            context.SetupGet(x => x.HttpBody).Returns("Meine Nachricht");
            context.SetupGet(x => x.HttpRequest).Returns(new List<string>() {"messages","1"});
            context.SetupGet(x => x.HttpMethod).Returns(HttpMethods.POST);
            
            MessageModel messageModel = new MessageModel();
            MessageHandler handler = new MessageHandler(context.Object,messageModel);

            var response = handler.Handle();
            
            Assert.That(response.StatusCode == StatusCodes.Created && response.ResponseMessage.Count == 1);
        }
        
        [Test]
        public void DeleteMessage()
        {
            Mock<IRequestContext> context = new Mock<IRequestContext>();
            context.SetupGet(x => x.HttpBody).Returns("Meine Nachricht");
            context.SetupGet(x => x.HttpRequest).Returns(new List<string>() {"messages","1"});
            context.SetupGet(x => x.HttpMethod).Returns(HttpMethods.DELETE);
            
            MessageModel messageModel = new MessageModel();
            messageModel.Add("test");
            MessageHandler handler = new MessageHandler(context.Object,messageModel);

            var response = handler.Handle();
            
            Assert.That(response.StatusCode == StatusCodes.OK && response.ResponseMessage[0].Id == 1);
        }
        
        [Test]
        public void PutMessage()
        {
           

            Mock<IRequestContext> context = new Mock<IRequestContext>();
            context.SetupGet(x => x.HttpBody).Returns("Meine Nachricht");
            context.SetupGet(x => x.HttpRequest).Returns(new List<string>() {"messages","1"});
            context.SetupGet(x => x.HttpMethod).Returns(HttpMethods.PUT);
            
            MessageModel messageModel = new MessageModel();
            messageModel.Add("test");
            MessageHandler handler = new MessageHandler(context.Object,messageModel);

            var response = handler.Handle();
            
            Assert.That(response.StatusCode == StatusCodes.OK 
                        && response.ResponseMessage[0].Id== 1);
        } 
    }
}