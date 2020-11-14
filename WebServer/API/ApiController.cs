using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Serilog;
using WebServer.Interface;
using WebServer.Model;
using WebServer.RessourcenHandler;
using TcpClient = WebServer.Model.TcpClient;

namespace WebServer.API
{
    /// <summary>
    /// Class that Controls requestContext & responseContext by using MessageModel
    /// </summary>
    public class ApiController
    {
        /// <summary>
        /// HttpRequest Context
        /// </summary>
        private  RequestContext _requestContext;
        /// <summary>
        /// HttpResponse 
        /// </summary>
        private  ResponseContext _responseContext;
        /// <summary>
        /// In Memory MessageList
        /// </summary>
        private static readonly MessageModel MsgModel = new MessageModel();
        /// <summary>
        /// Client that requested ressource
        /// </summary>
        private ITcpClient _client;


        public ApiController(ITcpClient client)
        {
            _client = client;
            ReceiveFromClient();
        }

        /// <summary>
        /// Reading from Client and passing into requestContext for parsing
        /// </summary>
        private void ReceiveFromClient()
        {
            string data = _client.ReadToEnd();

            Log.Debug($"Received:\r\n{data}");
            // Process the data sent by the client.
            
            _requestContext = new RequestContext();
            _requestContext.ParseRequestFromHeader(data);
        }

        /// <summary>
        /// Handles the Request and Respons to client
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">Throwen when a ressource is not recognized</exception>
        /// <exception cref="TargetParameterCountException">Wrong Parameter Count entered</exception>
        /// <exception cref="InvalidDataException">When no HttpBody is entered</exception>
        public virtual string CreateResponse()
        {
            _responseContext = new ResponseContext();
            try
            {
                switch (GetRequestedEndPoint())
                {
                    case ApiFunctionNames.messages:
                        MessageHandler handler = new MessageHandler(_requestContext,MsgModel);
                        _responseContext = handler.Handle();
                        break;
                    default:
                        throw new NotImplementedException("Unknown Ressource");
                }
            }
            catch (Exception e)
            {
                _responseContext.ResponseMessage.Add(new ResponseMessage()
                {
                    ErrorMessage = e.Message,
                    Status = StatusCodes.InternalServerError
                });

                _responseContext.StatusCode = StatusCodes.InternalServerError;
            }

            return _responseContext.BuildResponse();
        }

        /// <summary>
        /// Writes message to client 
        /// </summary>
        /// <param name="msg">message to send to client</param>
        public void Respond(string msg)
        {
            var stream = _client.GetStream();
            stream.Write(Encoding.ASCII.GetBytes(msg));
        }

        /// <summary>
        /// Parse the Request to the Ressource Endpoint
        /// </summary>
        /// <returns></returns>
        protected virtual ApiFunctionNames GetRequestedEndPoint()
        {
            if (_requestContext.HttpRequest.Count <= 0 || !Enum.TryParse(_requestContext.HttpRequest[0], true, out ApiFunctionNames result))
                return ApiFunctionNames.unknown;

            return result;
        }
    }
}
