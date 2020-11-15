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
    public class ApiController : BaseApiController
    {
       
        /// <summary>
        /// In Memory MessageList
        /// </summary>
        private static readonly MessageModel MsgModel = new MessageModel();

        public ApiController(ITcpClient client)
        {
            _client = client;
            ReceiveFromClient();
        }
        
        /// <summary>
        /// Handles the Request and Respons to client
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">Throwen when a ressource is not recognized</exception>
        /// <exception cref="TargetParameterCountException">Wrong Parameter Count entered</exception>
        /// <exception cref="InvalidDataException">When no HttpBody is entered</exception>
        public override string CreateResponse()
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
                Log.Error(e.Message);
            }

            return _responseContext.BuildResponse();
        }
        
    }
}
