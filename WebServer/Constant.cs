using System;
using System.Collections.Generic;
using System.Text;

namespace WebServer
{
    public enum HttpMethods
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    public enum ApiFunctionNames
    {
        messages
    }

    class Constant
    {
        public const string ServerName = "Server_404 Not Found";
        public const string DefaultHttpVersion = "HTTP/1.1";
    }
}