using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Serilog;
using WebServer.API;

namespace WebServer
{
    public class MimeTypes
    {
        public const string HTML = "text/html; charset=UTF-8";
        public const string JSON = "application/json; charset=UTF-8";
    }

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

    public class ResponseContext
    {

        public string Mime { get; set; }
        public StatusCodes StatusCode { get; set; } = StatusCodes.OK;
        public List<ResponseMessage> ResponseMessage { get; set; }
        public string ServerName { get; set; }
        private int ContentLength { get; set; }
        private string HttpBody { get; set; }

        private string ResponseHeader { get; set; }


        public ResponseContext()
        {
            Mime = MimeTypes.JSON;
            StatusCode = StatusCodes.OK;
            ServerName = Constant.ServerName;
            ResponseMessage = new List<ResponseMessage>();
        }

        private void BuildHeader()
        {
            ResponseHeader = $"{Constant.DefaultHttpVersion} {(int) StatusCode}\r\n" +
                              $"Server: {ServerName}\r\n";

            if (ContentLength > 0)
                ResponseHeader += $"Content-Type: {Mime}\r\n"
                        +$"Content-Length: {ContentLength}\r\n\r\n";
        }

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
            
            BuildHeader();

            if (ContentLength == 0)
            {
                Log.Debug($"Response:\r\n{ResponseHeader}");
                return ResponseHeader;
            }
            
            Log.Debug($"Response:\r\n{ResponseHeader}\r\n {HttpBody}");
            return ResponseHeader + HttpBody;
        }
    }
}