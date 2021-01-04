using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Serilog;
using WebServer.API;
using WebServer.Interface;

namespace WebServer.API
{
    /// <summary>
    /// Mime Types for response
    /// </summary>
    public class MimeTypes
    {
        public const string JSON = "application/json; charset=UTF-8";
        public const string PLAINTEXT = "text/plain; charset=UTF-8";
    }

    /// <summary>
    /// ResponseContext for client
    /// </summary>
    public class ResponseContext : IResponseContext
    {

        /// <summary>
        /// MimeType of response
        /// </summary>
        public string Mime { get; set; }
        /// <summary>
        /// StatusCode for HttpResponse
        /// </summary>
        public StatusCodes StatusCode { get; set; } = StatusCodes.OK;
        /// <summary>
        /// List of Messages as attachment
        /// </summary>
        public List<ResponseMessage> ResponseMessage { get; set; }
        /// <summary>
        /// Name of our Webserver
        /// </summary>
        public string ServerName { get; set; }
        /// <summary>
        /// Length of HttpResponse Payload
        /// </summary>
        public int ContentLength { get; set; }
        /// <summary>
        /// Payload
        /// </summary>
        public string HttpBody { get; set; }
        /// <summary>
        /// HttpHeader
        /// </summary>

        public string ResponseHeader { get; set; }


        public ResponseContext()
        {
            Mime = MimeTypes.JSON;
            StatusCode = StatusCodes.OK;
            ServerName = Constant.ServerName;
            ResponseMessage = new List<ResponseMessage>();
            
            ResponseHeader = $"{Constant.DefaultHttpVersion} {(int) StatusCode}\r\n" +
                             $"Server: {ServerName}\r\n";
            
        }

        /// <summary>
        /// Returns a string containing header and payload if a payload exists
        /// </summary>
        /// <returns>string for httpResponse</returns>
        public string BuildResponse()
        {
            if (ResponseMessage.Count > 0)
            {
                HttpBody = JsonConvert.SerializeObject(ResponseMessage,Formatting.Indented,new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                ContentLength = HttpBody.Length;
            }

            if (ContentLength > 0)
                ResponseHeader += $"Content-Type: {Mime}\r\n"
                                  +$"Content-Length: {ContentLength}\r\n\r\n";

            if (ContentLength <= 0)
            {
                Log.Debug($"Response:\r\n{ResponseHeader}");
                return ResponseHeader;
            }
            
            Log.Debug($"Response:\r\n{ResponseHeader}\r\n {HttpBody}");
            return ResponseHeader + HttpBody;
        }
    }
}