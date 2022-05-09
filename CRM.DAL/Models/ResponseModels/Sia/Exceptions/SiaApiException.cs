using System;
using System.Net.Http;

namespace CRM.DAL.Models.ResponseModels.Sia.Exceptions
{
    [Serializable]
    public class SiaApiException : Exception
    {
        public readonly HttpResponseMessage FailedResponse;
        public SiaApiException ()
        {}

        public SiaApiException(HttpResponseMessage responseMessage):base(responseMessage.StatusCode.ToString())
        {
            FailedResponse = responseMessage;
        }
        
        public SiaApiException(HttpResponseMessage responseMessage, Exception innerException):base(responseMessage.StatusCode.ToString(),innerException)
        {
            FailedResponse = responseMessage;
        }
    
        public SiaApiException (string message) 
            : base(message)
        {}
    
        public SiaApiException (string message, Exception innerException)
            : base (message, innerException)
        {}    
    }
}