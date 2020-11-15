using System;
using System.Text;
using Serilog;
using WebServer.Interface;

namespace WebServer.API
{
    public abstract class BaseApiController
    {
        /// <summary>
        /// HttpRequest Context
        /// </summary>
        protected  RequestContext _requestContext;
        /// <summary>
        /// HttpResponse 
        /// </summary>
        protected  ResponseContext _responseContext;
        /// <summary>
        /// Client that requested ressource
        /// </summary>
        protected ITcpClient _client;

        /// <summary>
        /// Reading from Client and passing into requestContext for parsing
        /// </summary>
        protected virtual void ReceiveFromClient()
        {
            string data = _client.ReadToEnd();

            Log.Debug($"Received:\r\n{data}");
            // Process the data sent by the client.
            
            _requestContext = new RequestContext();
            _requestContext.ParseRequestFromHeader(data);
        }

        public abstract string CreateResponse();
        
        /// <summary>
        /// Writes message to client 
        /// </summary>
        /// <param name="msg">message to send to client</param>
        public virtual void Respond(string msg)
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