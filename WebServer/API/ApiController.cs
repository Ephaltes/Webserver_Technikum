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
    public class ApiController
    {
        private readonly RequestContext _requestContext;
        private readonly ResponseContext _responseContext;
        private static readonly MessageModel MsgController = new MessageModel();


        public ApiController(TcpClient client)
        {
            _requestContext = new RequestContext(client);
            _responseContext = new ResponseContext(client, _requestContext.IsBrowser);
        }

        public void ResponseToClient()
        {
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
                                Status = ResponseStatus.Success
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
                                    Status = ResponseStatus.Success,
                                    Id = id
                                });
                                _responseContext.StatusCode = StatusCodes.OK;
                            }
                            else
                            {
                                _responseContext.ResponseMessage.Add(new ResponseMessage()
                                {
                                    Status = ResponseStatus.Failed,
                                    Id = id,
                                    ErrorMessage = "MessageId not found"
                                });
                                _responseContext.StatusCode = StatusCodes.BadRequest;
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
                                    Status = ResponseStatus.Success,
                                    Id = id
                                });
                                _responseContext.StatusCode = StatusCodes.OK;
                            }

                            else
                            {
                                _responseContext.ResponseMessage.Add(new ResponseMessage()
                                {
                                    Status = ResponseStatus.Success,
                                    ErrorMessage = "Message not found",
                                    Id = id
                                });
                                _responseContext.StatusCode = StatusCodes.BadRequest;
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
                    Status = ResponseStatus.Failed
                });

                _responseContext.StatusCode = StatusCodes.NotImplemented;
            }


            _responseContext.ResponseToClient();
        }

        private ResponseMessage DisplayMessage(int id)
        {
            Message msg = MsgController.GetMessage(id);

            if (msg == null)
            {
                return new ResponseMessage()
                {
                    ErrorMessage = "Message not Found",
                    Status = ResponseStatus.Failed
                };
            }

            return new ResponseMessage()
            {
                Object = msg,
                Status = ResponseStatus.Success
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
                    Status = ResponseStatus.Failed
                });
                return ret;
            }


            foreach (Message msg in msgList)
            {

                ret.Add(new ResponseMessage()
                {
                    Status = ResponseStatus.Success,
                    Object = msg,
                });
            }
            return ret;
        }

    }
}
