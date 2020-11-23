using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Serilog;

namespace WebServer.Interface
{
   
    public interface IRequestContext
    {
        public string HttpVersion { get; }
        public HttpMethods HttpMethod { get; }
        public List<string> HttpRequest { get; }
        public Dictionary<string, string> HttpHeader { get; }
        public string HttpBody { get; }

        public int ContentLength { get; }

        protected const int HTTPVERBPOS = 0;

        
        public bool ParseRequestFromHeader(string httpRequest);

        public int GetContentLength();
    }
}
