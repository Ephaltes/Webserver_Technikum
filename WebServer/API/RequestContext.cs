using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Serilog;

namespace WebServer
{

    class RequestContext
    {
        public string HttpVersion { get; set; }
        public HttpMethods HttpMethod { get; set; }
        public string HttpRequest { get; set; }
        public Dictionary<string, string> HttpHeader { get; set; }
        public string HttpBody { get; set; }


        private const int HTTPVERB = 0;

        public RequestContext(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            string data = "";
            do
            {
                Byte[] bytes = new Byte[4096];
                int i = stream.Read(bytes, 0, bytes.Length);
                // Translate data bytes to a ASCII string.
                data += System.Text.Encoding.ASCII.GetString(bytes, 0, i);
            } while (stream.DataAvailable);

            Log.Debug($"Received:\r\n{data}");
            // Process the data sent by the client.
            data = data.ToUpper();
            string[] splittedData = data.Split("\r\n");
            ParseRequestFromHeader(splittedData);
        }

        private void ParseRequestFromHeader(string[] header)
        {
            string[] splittedverb = header[HTTPVERB].Split(" ");
            Enum.TryParse(splittedverb[0], out HttpMethods methodTemp);
            HttpMethod = methodTemp;
            HttpRequest = splittedverb[1];
            HttpVersion = splittedverb[2];
            HttpBody = header.Last();

            HttpHeader = new Dictionary<string, string>();

            for (int i = 1; i < header.Length - 1; i++)
            {
                if (String.IsNullOrWhiteSpace(header[i]))
                    continue;

                string[] temp = header[i].Split(":", 2);
                HttpHeader.Add(temp[0], temp[1]);
            }

        }

        private void GetRequestFromHeader(string httpverb)
        {
            HttpRequest = httpverb.Split(" ")[1];
        }

        private void GetHttpMethodFromHeader(string httpverb)
        {
            if (httpverb.Contains(HttpMethods.GET.ToString()))
            {
                HttpMethod = HttpMethods.GET;
                return;
            }

            if (httpverb.Contains(HttpMethods.POST.ToString()))
            {
                HttpMethod = HttpMethods.POST;
                return;
            }

            if (httpverb.Contains(HttpMethods.PUT.ToString()))
            {
                HttpMethod = HttpMethods.PUT;
                return;
            }

            if (httpverb.Contains(HttpMethods.DELETE.ToString()))
            {
                HttpMethod = HttpMethods.DELETE;
            }
        }

    }
}
