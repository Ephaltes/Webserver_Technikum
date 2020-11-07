using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Serilog;

namespace WebServer
{

    public class RequestContext
    {
        public string HttpVersion { get; set; }
        public HttpMethods HttpMethod { get; set; }
        public string HttpRequest { get; set; }
        public Dictionary<string, string> HttpHeader { get; set; }
        public string HttpBody { get; set; }

        private const int HTTPVERB = 0;

        public RequestContext(string httpRequest)
        {
            string[] splittedData = httpRequest.Split("\r\n");
            ParseRequestFromHeader(splittedData);
        }

        private void ParseRequestFromHeader(string[] header)
        {
            string[] splittedverb = header[HTTPVERB].Split(" ");
            Enum.TryParse(splittedverb[0], out HttpMethods methodTemp);
            HttpMethod = methodTemp;
            HttpRequest = splittedverb[1];
            HttpVersion = splittedverb[2];


            HttpHeader = new Dictionary<string, string>();

            bool isBody = false;

            for (int i = 1; i < header.Length; i++)
            {
                if (String.IsNullOrWhiteSpace(header[i]))
                {
                    isBody = true;
                    continue;
                }

                if (!isBody)
                {
                    string[] temp = header[i].Split(":", 2);
                    HttpHeader.Add(temp[0], temp[1]);
                }
                else
                {
                    if (!string.IsNullOrEmpty(HttpBody))
                        HttpBody += "\r\n";
                    HttpBody += $"{header[i]}";
                }

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
