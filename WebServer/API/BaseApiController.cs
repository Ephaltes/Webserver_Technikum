using System;
using System.Collections.Generic;
using System.Linq;
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
        protected  IRequestContext _requestContext;
        /// <summary>
        /// HttpResponse 
        /// </summary>
        protected  IResponseContext _responseContext;
        /// <summary>
        /// Client that requested ressource
        /// </summary>
        protected ITcpClient _client;

        protected List<string> _endpointList;

        /// <summary>
        /// Reading from Client and passing into requestContext for parsing
        /// </summary>
        protected virtual string ReceiveFromClient()
        {
            string data = _client.ReadToEnd();

            Log.Debug($"Received:\r\n{data}");
            // Process the data sent by the client.

            return data;
        }

        public abstract string ForwardToEndPointHandler();
        
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
        /// <returns>EndPointName</returns>
        protected virtual string GetRequestedEndPoint()
        {
            if (_requestContext.HttpRequest.Count <= 0 || 
                !_endpointList.Contains(_requestContext.HttpRequest[0],StringComparer.OrdinalIgnoreCase))
                return "";

            return _requestContext.HttpRequest[0];
        }
    }
}