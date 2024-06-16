using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Common.Exceptions
{
    public class AppException : Exception
    {
        public ErrorResponse.ErrorEnum ErrCode { get; set; }

        public AppException(string details, Exception ex = null, ILogger logger = null) : base(details, ex)
        {
            if (logger != null)
            {
                logger.LogError(Message);
            }
            ErrCode = ErrorResponse.ErrorEnum.BadRequest;
        }

        public AppException(ErrorResponse.ErrorEnum errorCode, string details = null, Exception ex = null, ILogger logger = null, bool rawMsg = false) : base(rawMsg ? details : (ErrorResponse.GetErrorMessage(errorCode) + ": " + details), ex)
        {
            if (logger != null)
            {
                logger.LogError(Message);
            }
            ErrCode = errorCode;
        }
    }
}
