using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using WebServer.API;
using WebServer.Interface;
using WebServer.Model;

namespace WebServer.RessourcenHandler
{
    public class MessageHandler : BaseRessourceHandler
    {

        private RequestContext _requestContext;
        private MessageModel _messageModel;
        public MessageHandler(RequestContext req, MessageModel model)
        {
            _requestContext = req;
            _messageModel = model;
        }
        /// <summary>
        /// Wrapper Function that handles all HttpVerbs
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">is thrown when HttpVerb is not implemented</exception>
        public override  ResponseContext Handle()
        {
            ResponseContext responseContext;
            switch (_requestContext.HttpMethod)
                {
                    case HttpMethods.GET:
                        responseContext = HandleGet();
                    break;
                    case HttpMethods.POST:
                        responseContext = HandlePost();
                            break;
                    case HttpMethods.PUT:
                        responseContext = HandlePut();
                        
                            break;
                    case HttpMethods.DELETE:
                        responseContext = HandleDelete();
                            break;
                    default:
                        throw new NotImplementedException("HttpMethod not Implemented");
                }
            return responseContext;
        }

        /// <summary>
        /// Displays a single message 
        /// </summary>
        /// <param name="id">id of Message to be shown</param>
        /// <returns>Message object</returns>
        private ResponseMessage DisplayMessage(int id)
        {
            Message msg = _messageModel.GetMessage(id);

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

        /// <summary>
        /// Get a list of all messages
        /// </summary>
        /// <returns>List of messages</returns>
        private List<ResponseMessage> DisplayMessages()
        {
            List<Message> msgList = _messageModel.GetMessages();
            List<ResponseMessage> ret = new List<ResponseMessage>();

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

        protected override ResponseContext HandleGet()
        {
            ResponseContext responseContext = new ResponseContext();
            if (_requestContext.HttpRequest.Count == 1)
            {
                responseContext.ResponseMessage = DisplayMessages();
            }
            else if (_requestContext.HttpRequest.Count == 2)
            {
                responseContext.ResponseMessage.Add(DisplayMessage(Convert.ToInt32(_requestContext.HttpRequest[1])));
            }
            else
                throw new TargetParameterCountException("too many argument received");

            return responseContext;
        }

        protected override  ResponseContext HandlePost()
        {
            ResponseContext responseContext = new ResponseContext();
            if (String.IsNullOrWhiteSpace(_requestContext.HttpBody))
                throw new InvalidDataException("No HttpBody received for Post");

            responseContext.ResponseMessage.Add(new ResponseMessage()
            {
                Object = _messageModel.Add(_requestContext.HttpBody),
                Status = StatusCodes.Created
            });
            responseContext.StatusCode = StatusCodes.Created;
            return responseContext;
        }

        protected override  ResponseContext HandlePut()
        {
            ResponseContext responseContext = new ResponseContext();
            if (String.IsNullOrWhiteSpace(_requestContext.HttpBody))
                throw new InvalidDataException("No HttpBody received for Put");

            int id = Convert.ToInt32(_requestContext.HttpRequest[1]);
            if (_messageModel.Update(id, _requestContext.HttpBody))
            {
                responseContext.ResponseMessage.Add(new ResponseMessage()
                {
                    Status = StatusCodes.OK,
                    Id = id
                });
                responseContext.StatusCode = StatusCodes.OK;
            }
            else
            {
                responseContext.ResponseMessage.Add(new ResponseMessage()
                {
                    Status = StatusCodes.NotFound,
                    Id = id,
                    ErrorMessage = "MessageId not found"
                });
                responseContext.StatusCode = StatusCodes.NotFound;
            }
            return responseContext;
        }

        protected override  ResponseContext HandleDelete()
        {
            ResponseContext responseContext = new ResponseContext();
            int id = Convert.ToInt32(_requestContext.HttpRequest[1]);
            if (_messageModel.Delete(id))
            {
                responseContext.ResponseMessage.Add(new ResponseMessage()
                {
                    Status = StatusCodes.OK,
                    Id = id
                });
                responseContext.StatusCode = StatusCodes.OK;
            }

            else
            {
                responseContext.ResponseMessage.Add(new ResponseMessage()
                {
                    Status = StatusCodes.NotFound,
                    ErrorMessage = "Message not found",
                    Id = id
                });
                responseContext.StatusCode = StatusCodes.NotFound;
            }
            return responseContext;
        }
    }
}