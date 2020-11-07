using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WebServer
{
    enum HttpMethod
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    class RequestContext
    {
        public string HttpVersion { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public string HttpRequest { get; set; }
        public Dictionary<string, string> HttpHeader { get; set; }
        public string HttpPayload { get; set; }


        private const int HTTPVERB = 0;

        public RequestContext(TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            if (stream.DataAvailable)
            {
                Byte[] bytes = new Byte[4096];
                int i = stream.Read(bytes, 0, bytes.Length);
                // Translate data bytes to a ASCII string.
                string data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("Received: {0}", data);

                // Process the data sent by the client.
                data = data.ToUpper();
                string[] splittedData = data.Split("\r\n");
                ParseRequestFromHeader(splittedData);

            }
        }

        private void ParseRequestFromHeader(string[] header)
        {
            string[] splittedverb = header[HTTPVERB].Split(" ");
            Enum.TryParse(splittedverb[0], out HttpMethod methodTemp);
            HttpMethod = methodTemp;
            HttpRequest = splittedverb[1];
            HttpVersion = splittedverb[2];
            HttpPayload = header.Last();

            HttpHeader = new Dictionary<string, string>();

            for (int i = 1; i < header.Length-1; i++)
            {
                if (String.IsNullOrWhiteSpace(header[i]))
                    continue;

                string[] temp = header[i].Split(":",2);
                HttpHeader.Add(temp[0],temp[1]);
            }

        }

        private void GetRequestFromHeader(string httpverb)
        {
            HttpRequest = httpverb.Split(" ")[1];
        }

        private void GetHttpMethodFromHeader(string httpverb)
        {
            if (httpverb.Contains(HttpMethod.GET.ToString()))
            {
                HttpMethod = HttpMethod.GET;
                return;
            }

            if (httpverb.Contains(HttpMethod.POST.ToString()))
            {
                HttpMethod = HttpMethod.POST;
                return;
            }

            if (httpverb.Contains(HttpMethod.PUT.ToString()))
            {
                HttpMethod = HttpMethod.PUT;
                return;
            }

            if (httpverb.Contains(HttpMethod.DELETE.ToString()))
            {
                HttpMethod = HttpMethod.DELETE;
            }
        }

    }
}
