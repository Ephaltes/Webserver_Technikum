using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Serilog;
using WebServer.Interface;

namespace WebServer.API
{

    /// <summary>
    /// Class for receiving HttpRequest
    /// </summary>
    public class RequestContext : IRequestContext
    {
        /// <summary>
        /// HttpVersion in request
        /// </summary>
        public string HttpVersion { get; protected set; }
        /// <summary>
        /// HttpVerb
        /// </summary>
        public HttpMethods HttpMethod { get; protected set; }
        /// <summary>
        /// Ressource target
        /// </summary>
        public List<string> HttpRequest { get; protected set; }
        /// <summary>
        /// Http header except First row
        /// </summary>
        public Dictionary<string, string> HttpHeader { get; protected set; }
        /// <summary>
        /// HttpPayload
        /// </summary>
        public string HttpBody { get; protected set; }

        public int ContentLength { get;protected set; }

        protected const int HTTPVERBPOS = 0;


        public RequestContext()
        {
            HttpRequest = new List<string>();
            HttpHeader = new Dictionary<string, string>();
        }

        /// <summary>
        /// Parsing Http key value pairs from header
        /// </summary>
        /// <param name="httpRequest">/HttpRequest</param>
        public bool ParseRequestFromHeader(string httpRequest)
        {
            string[] header = httpRequest.Split("\r\n");

            if (header.Length < 1)
                return false;
            
            string[] splittedverb = header[HTTPVERBPOS].Split(" ");

            if (splittedverb.Length != 3)
                return false;
            
            Enum.TryParse(splittedverb[0], out HttpMethods methodTemp);
            HttpMethod = methodTemp;
            HttpRequest = splittedverb[1].Split("/",StringSplitOptions.RemoveEmptyEntries).ToList();
            HttpVersion = splittedverb[2];

            bool isBody = false;

            for (int i = 1; i < header.Length; i++)
            {
                if (String.IsNullOrWhiteSpace(header[i]))
                {
                    isBody = true;
                    ContentLength = GetContentLength();
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
            if(ContentLength>0 && ContentLength >= HttpBody.Length)
                HttpBody = HttpBody.Substring(0,ContentLength);
            return true;
        }
        
        public int GetContentLength()
        {
            if (HttpHeader.ContainsKey("Content-Length"))
                return Int32.Parse(HttpHeader["Content-Length"]);

            return 0;
        }
    }
}
