using System;
using System.Collections.Generic;
using System.Text;

namespace WebServer.API
{
    public enum ResponseStatus
    {
        Success=100,
        Failed=400
    }

    public class ResponseMessage
    {
        public string ErrorMessage;
        public object Object;
        public ResponseStatus Status=ResponseStatus.Success;
        public int? Id;

    }
}