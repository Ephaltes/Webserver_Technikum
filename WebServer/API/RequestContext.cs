using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Serilog;

namespace WebServer
{

    /// <summary>
    /// Class for receiving HttpRequest
    /// </summary>
    public class RequestContext
    {
        /// <summary>
        /// HttpVersion in request
        /// </summary>
        public string HttpVersion { get; set; }
        /// <summary>
        /// HttpVerb
        /// </summary>
        public HttpMethods HttpMethod { get; set; }
        /// <summary>
        /// Ressource target
        /// </summary>
        public List<string> HttpRequest { get; set; }
        /// <summary>
        /// Http header except First row
        /// </summary>
        public Dictionary<string, string> HttpHeader { get; set; }
        /// <summary>
        /// HttpPayload
        /// </summary>
        public string HttpBody { get; set; }

        private const int HTTPVERB = 0;

        /// <summary>
        /// Parsing Http key value pairs from header
        /// </summary>
        /// <param name="httpRequest">/HttpRequest</param>
        public void ParseRequestFromHeader(string httpRequest)
        {
            string[] header = httpRequest.Split("\r\n");
            
            string[] splittedverb = header[HTTPVERB].Split(" ");
            Enum.TryParse(splittedverb[0], out HttpMethods methodTemp);
            HttpMethod = methodTemp;
            HttpRequest = splittedverb[1].Split("/",StringSplitOptions.RemoveEmptyEntries).ToList();
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
    }
}
