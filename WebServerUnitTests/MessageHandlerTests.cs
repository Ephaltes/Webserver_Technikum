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
using WebServer.RessourcenHandler;

namespace WebServerUnitTests
{
    public class MessageHandlerTests
    {
        [Test]
        public void GetMessages()
        {
            RequestContext context = new RequestContext() 
                {
                    HttpBody = "Meine Nachricht" , 
                    HttpRequest = new List<string>() {"messages"},
                    HttpMethod = HttpMethods.GET,HttpVersion = "Http/4.0"
                };
            MessageModel messageModel = new MessageModel();
            MessageHandler handler = new MessageHandler(context,messageModel);

            var response = handler.Handle();
            
            Assert.That(response.StatusCode == StatusCodes.OK);
        }
        
        [Test]
        public void PostMessage()
        {
            RequestContext context = new RequestContext() 
            {
                HttpBody = "Meine Nachricht" , 
                HttpRequest = new List<string>() {"messages"},
                HttpMethod = HttpMethods.POST,HttpVersion = "Http/4.0"
            };
            MessageModel messageModel = new MessageModel();
            MessageHandler handler = new MessageHandler(context,messageModel);

            var response = handler.Handle();
            
            Assert.That(response.StatusCode == StatusCodes.Created && response.ResponseMessage.Count == 1);
        }
        
        [Test]
        public void DeleteMessage()
        {
            RequestContext context = new RequestContext() 
            {
                HttpBody = "Meine Nachricht" , 
                HttpRequest = new List<string>() {"messages","1"},
                HttpMethod = HttpMethods.DELETE,HttpVersion = "Http/4.0"
            };
            MessageModel messageModel = new MessageModel();
            messageModel.Add("test");
            MessageHandler handler = new MessageHandler(context,messageModel);

            var response = handler.Handle();
            
            Assert.That(response.StatusCode == StatusCodes.OK && response.ResponseMessage[0].Id == 1);
        }
        
        [Test]
        public void PutMessage()
        {
           

            RequestContext context = new RequestContext() 
            {
                HttpBody = "Meine Nachricht" , 
                HttpRequest = new List<string>() {"messages","1"},
                HttpMethod = HttpMethods.PUT,HttpVersion = "Http/4.0"
            };
            MessageModel messageModel = new MessageModel();
            messageModel.Add("test");
            MessageHandler handler = new MessageHandler(context,messageModel);

            var response = handler.Handle();
            
            Assert.That(response.StatusCode == StatusCodes.OK 
                        && response.ResponseMessage[0].Id== 1);
        }
    }
}