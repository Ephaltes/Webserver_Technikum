using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using WebServer.Model;

namespace WebServer.API
{
    class ApiController
    {
        private readonly RequestContext _requestContext;
        private readonly ResponseContext _responseContext;
        private static readonly MessageController MsgController = new MessageController();


        public ApiController(TcpClient client)
        {
            _requestContext = new RequestContext(client);
            _responseContext = new ResponseContext(client,_requestContext.IsBrowser);
        }

        public void ResponseToClient()
        {
            try
            {
                var splitted = _requestContext.HttpRequest.Split("/");
                List<string> splittedRequest = splitted.Where(a => !string.IsNullOrWhiteSpace(a)).ToList();

                if (splittedRequest.Count == 0 ||
                    !Enum.IsDefined(typeof(ApiFunctionNames), splittedRequest[0].ToLower()))
                    throw new NotImplementedException("Function is not Implemented\r\n");


                switch (_requestContext.HttpMethod)
                {
                    case HttpMethods.GET:

                        if (splittedRequest[0].ToLower() == ApiFunctionNames.messages.ToString())
                        {
                            if (splittedRequest.Count == 1)
                            {

                                _responseContext.HttpBody = DisplayMessages();
                            }
                            else if (splittedRequest.Count == 2)
                            {
                                _responseContext.HttpBody = DisplayMessage(Convert.ToInt32(splittedRequest[1]));
                            }
                            else
                                throw new TargetParameterCountException("too many argument received\r\n");
                        }

                        break;
                    case HttpMethods.POST:
                        if (splittedRequest[0].ToLower() == ApiFunctionNames.messages.ToString())
                        {
                            if (String.IsNullOrWhiteSpace(_requestContext.HttpBody))
                                throw new InvalidDataException("No HttpBody received for Post\r\n");

                            _responseContext.HttpBody =
                                $"Created new Message with Id: {MsgController.Add(_requestContext.HttpBody)}";
                            _responseContext.StatusCode = StatusCodes.Created;
                        }

                        break;
                    case HttpMethods.PUT:
                        if (splittedRequest[0].ToLower() == ApiFunctionNames.messages.ToString())
                        {
                            if (String.IsNullOrWhiteSpace(_requestContext.HttpBody))
                                throw new InvalidDataException("No HttpBody received for Post\r\n");

                            int id = Convert.ToInt32(splittedRequest[1]);
                            if (MsgController.Update(id, _requestContext.HttpBody))
                            {
                                _responseContext.HttpBody = $"Updated Message with Id: {id} \r\n";
                            }
                            else
                                _responseContext.HttpBody = "Message not found \r\n";

                        }

                        break;
                    case HttpMethods.DELETE:
                        if (splittedRequest[0].ToLower() == ApiFunctionNames.messages.ToString())
                        {
                            int id = Convert.ToInt32(splittedRequest[1]);
                            if (MsgController.Delete(id))
                                _responseContext.HttpBody = $"Deleted Message with Id: {id}\r\n";
                            else
                                _responseContext.HttpBody = "Message not found\r\n";
                        }

                        break;
                    default:
                        throw new NotImplementedException("HttpMethod not Implemented\r\n");

                }
            }
            catch (Exception e)
            {
                _responseContext.HttpBody = $"Error: {e.Message}\r\n {e.InnerException?.Message}\r\n";
                _responseContext.StatusCode = StatusCodes.NotImplemented;
            }


            _responseContext.ResponseToClient();
        }

        private string DisplayMessage(int id)
        {
            Message msg = MsgController.GetMessage(id);

            if (msg == null)
                return "Message not Found\r\n";

            return $"id: {msg.id}\r\nMessage: {msg.message}\r\n";
        }

        private string DisplayMessages()
        {
            List<Message> msgList = MsgController.GetMessages();
            if (msgList.Count == 0)
            {
                return "No Messages Found\r\n";
            }

            string ret = "";
            foreach (Message msg in msgList)
            {
                string temp = $"id: {msg.id}\r\nMessage: {msg.message}\r\n\r\n";
                ret += temp;
            }
            return ret + "\r\n";
        }

    }
}
