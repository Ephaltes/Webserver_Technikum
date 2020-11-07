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
    /// <summary>
    /// Api Endpoint ressources
    /// </summary>

    public enum ApiFunctionNames
    {
        unknown,
        messages,
    }

    /// <summary>
    /// Server Constants
    /// </summary>
    public class Constant
    {
        public const string ServerName = "Server_404 Not Found";
        public const string DefaultHttpVersion = "HTTP/1.1";
    }
    
    /// <summary>
    /// Http StatusCodes
    /// </summary>
    public enum StatusCodes
    {
        OK = 200,
        Created=201,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        InternalServerError = 500,
        NotImplemented = 501
    }
}