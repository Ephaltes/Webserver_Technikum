using System;
using System.Collections.Generic;
using System.Text;

namespace WebServer.API
{
    /// <summary>
    /// Entity for Response
    /// </summary>
    public class ResponseMessage
    {
        /// <summary>
        /// If there is an ErrorMessage
        /// </summary>
        public string ErrorMessage;
        /// <summary>
        /// Object to be send as response
        /// </summary>
        public object Object;
        /// <summary>
        /// Whether the execution was successfull
        /// </summary>
        public StatusCodes Status=StatusCodes.OK;
        /// <summary>
        /// Id of target
        /// </summary>
        public int? Id;

    }
}