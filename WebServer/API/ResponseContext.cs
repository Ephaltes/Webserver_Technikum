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
    }

    enum StatusCodes
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

    class ResponseContext
    {

        public byte[] ResponseHeader { get; set; }
        public TcpClient Client { get; set; }
        public string Mime { get; set; }
        public StatusCodes StatusCode { get; set; } = StatusCodes.OK;
        public List<ResponseMessage> ResponseMessage { get; set; }
        public string ServerName { get; set; }
        private int ContentLength { get; set; }
        private string HttpBody { get; set; }

        private bool IsBrowser;

        private string sResponseHeader { get; set; }


        public ResponseContext(TcpClient client, bool browser = true)
        {
            Client = client;
            Mime = MimeTypes.HTML;
            StatusCode = StatusCodes.OK;
            ServerName = Constant.ServerName;
            IsBrowser = browser;
            ResponseMessage = new List<ResponseMessage>();
        }

        private void BuildResponse()
        {
            sResponseHeader = $"{Constant.DefaultHttpVersion} {(int) StatusCode}\r\n" +
                              $"Server: {ServerName}\r\n";

            if (ContentLength > 0)
                sResponseHeader += $"Content-Type: {Mime}\r\n"
                        +$"Content-Length: {ContentLength}\r\n\r\n";

            ResponseHeader = Encoding.ASCII.GetBytes(sResponseHeader);

        }

        private void SendHeader()
        {
            BuildResponse();
            NetworkStream stream = Client.GetStream();
            stream.Write(ResponseHeader);
        }

        public void ResponseToClient()
        {
            if (ResponseMessage.Count > 0)
            {
                HttpBody = JsonConvert.SerializeObject(ResponseMessage,Formatting.Indented,new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }

            if (String.IsNullOrWhiteSpace(HttpBody))
            {
                SendHeader();
                Log.Debug($"Response:\r\n{sResponseHeader}");
                return;
            }

            //if(IsBrowser)
              //  ConvertLineBreakToBrowser();
            byte[] data = Encoding.ASCII.GetBytes(HttpBody);
            ContentLength = HttpBody.Length;
            SendHeader();


            NetworkStream stream = Client.GetStream();
            stream.Write(data);
            Log.Debug($"Response:\r\n{sResponseHeader}\r\n {Encoding.ASCII.GetString(data)}");
        }

        private void ConvertLineBreakToBrowser()
        {
           HttpBody = HttpBody.Replace("\r\n","<br>");
        }

    }
}