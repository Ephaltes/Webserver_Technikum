using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Serilog;
using WebServer.API;

namespace WebServer.Interface
{
   
    public interface IResponseContext
    {
        public string Mime { get; set; }
        public StatusCodes StatusCode { get; set; } 
        public List<ResponseMessage> ResponseMessage { get; set; }
        public string ServerName { get; set; }
        public int ContentLength { get; set; }
        public string HttpBody { get; set; }
        public string ResponseHeader { get; set; }

        public string BuildResponse();
    }
}
