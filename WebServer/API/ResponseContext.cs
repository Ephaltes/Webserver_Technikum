using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Serilog;

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
        public StatusCodes StatusCode { get; set; }
        public string ServerName { get; set; }
        private int ContentLength { get; set; }
        public string HttpBody { get; set; }

        private string sResponseHeader { get; set; }


        public ResponseContext(TcpClient client)
        {
            Client = client;
            Mime = MimeTypes.HTML;
            StatusCode = StatusCodes.OK;
            ServerName = Constant.ServerName;
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
            if (String.IsNullOrWhiteSpace(HttpBody))
            {
                SendHeader();
                Log.Debug($"Response:\r\n{sResponseHeader}");
                return;
            }
            byte[] data = Encoding.ASCII.GetBytes(HttpBody);
            ContentLength = HttpBody.Length;
            SendHeader();


            NetworkStream stream = Client.GetStream();
            stream.Write(data);
            Log.Debug($"Response:\r\n{sResponseHeader}\r\n {Encoding.ASCII.GetString(data)}");
        }

    }
}