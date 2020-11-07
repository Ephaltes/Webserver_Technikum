using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Serilog;
using WebServer.Model;

namespace WebServer.API
{
    public class ApiController
    {
        private  RequestContext _requestContext;
        private  ResponseContext _responseContext;
        private static readonly MessageModel MsgController = new MessageModel();
        private TcpClient _client;


        public ApiController(TcpClient client)
        {
            _client = client;
        }

        public void ReceiveFromClient()
        {
            var stream = _client.GetStream();
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
            
            _requestContext = new RequestContext(data);
        }

        public void ResponseToClient()
        {
            _responseContext = new ResponseContext();
            try
            {
                var splitted = _requestContext.HttpRequest.Split("/");
                List<string> splittedRequest = splitted.Where(a => !string.IsNullOrWhiteSpace(a)).ToList();

                if (splittedRequest.Count == 0 ||
                    !Enum.IsDefined(typeof(ApiFunctionNames), splittedRequest[0].ToLower()))
                    throw new NotImplementedException("Function is not Implemented");

                switch (_requestContext.HttpMethod)
                {
                    case HttpMethods.GET:

                        if (splittedRequest[0].ToLower() == ApiFunctionNames.messages.ToString())
                        {
                            if (splittedRequest.Count == 1)
                            {
                                _responseContext.ResponseMessage = DisplayMessages();
                            }
                            else if (splittedRequest.Count == 2)
                            {
                                _responseContext.ResponseMessage.Add(DisplayMessage(Convert.ToInt32(splittedRequest[1])));
                            }
                            else
                                throw new TargetParameterCountException("too many argument received");
                        }

                        break;
                    case HttpMethods.POST:
                        if (splittedRequest[0].ToLower() == ApiFunctionNames.messages.ToString())
                        {
                            if (String.IsNullOrWhiteSpace(_requestContext.HttpBody))
                                throw new InvalidDataException("No HttpBody received for Post");

                            _responseContext.ResponseMessage.Add(new ResponseMessage()
                            {
                                Object = MsgController.Add(_requestContext.HttpBody),
                                Status = StatusCodes.Created
                            });
                            _responseContext.StatusCode = StatusCodes.Created;
                        }

                        break;
                    case HttpMethods.PUT:
                        if (splittedRequest[0].ToLower() == ApiFunctionNames.messages.ToString())
                        {
                            if (String.IsNullOrWhiteSpace(_requestContext.HttpBody))
                                throw new InvalidDataException("No HttpBody received for Post");

                            int id = Convert.ToInt32(splittedRequest[1]);
                            if (MsgController.Update(id, _requestContext.HttpBody))
                            {
                                _responseContext.ResponseMessage.Add(new ResponseMessage()
                                {
                                    Status = StatusCodes.OK,
                                    Id = id
                                });
                                _responseContext.StatusCode = StatusCodes.OK;
                            }
                            else
                            {
                                _responseContext.ResponseMessage.Add(new ResponseMessage()
                                {
                                    Status = StatusCodes.NotFound,
                                    Id = id,
                                    ErrorMessage = "MessageId not found"
                                });
                                _responseContext.StatusCode = StatusCodes.NotFound;
                            }

                        }

                        break;
                    case HttpMethods.DELETE:
                        if (splittedRequest[0].ToLower() == ApiFunctionNames.messages.ToString())
                        {
                            int id = Convert.ToInt32(splittedRequest[1]);
                            if (MsgController.Delete(id))
                            {
                                _responseContext.ResponseMessage.Add(new ResponseMessage()
                                {
                                    Status = StatusCodes.OK,
                                    Id = id
                                });
                                _responseContext.StatusCode = StatusCodes.OK;
                            }

                            else
                            {
                                _responseContext.ResponseMessage.Add(new ResponseMessage()
                                {
                                    Status = StatusCodes.NotFound,
                                    ErrorMessage = "Message not found",
                                    Id = id
                                });
                                _responseContext.StatusCode = StatusCodes.NotFound;
                            }
                        }
                        break;
                    default:
                        throw new NotImplementedException("HttpMethod not Implemented");

                }
            }
            catch (Exception e)
            {
                _responseContext.ResponseMessage.Add(new ResponseMessage()
                {
                    ErrorMessage = e.Message,
                    Status = StatusCodes.NotImplemented
                });

                _responseContext.StatusCode = StatusCodes.NotImplemented;
            }


            string response = _responseContext.BuildResponse();
            var stream = _client.GetStream();
            stream.Write(Encoding.ASCII.GetBytes(response));
        }

        private ResponseMessage DisplayMessage(int id)
        {
            Message msg = MsgController.GetMessage(id);

            if (msg == null)
            {
                return new ResponseMessage()
                {
                    ErrorMessage = "Message not Found",
                    Status = StatusCodes.BadRequest
                };
            }

            return new ResponseMessage()
            {
                Object = msg,
                Status = StatusCodes.OK
            };
        }

        private List<ResponseMessage> DisplayMessages()
        {
            List<Message> msgList = MsgController.GetMessages();
            List<ResponseMessage> ret = new List<ResponseMessage>();

            if (msgList.Count == 0)
            {
                ret.Add(new ResponseMessage()
                {
                    ErrorMessage = "Message not Found",
                    Status = StatusCodes.NotFound
                });
                return ret;
            }


            foreach (Message msg in msgList)
            {

                ret.Add(new ResponseMessage()
                {
                    Status = StatusCodes.OK,
                    Object = msg,
                });
            }
            return ret;
        }

    }
}
