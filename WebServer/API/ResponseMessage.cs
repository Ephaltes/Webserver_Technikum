using System;
using System.Collections.Generic;
using System.Text;

namespace WebServer.API
{

    public class ResponseMessage
    {
        public string ErrorMessage;
        public object Object;
        public StatusCodes Status=StatusCodes.OK;
        public int? Id;

    }
}