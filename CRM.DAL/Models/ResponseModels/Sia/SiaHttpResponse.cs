using System.Collections.Generic;
using System.Net;

namespace CRM.DAL.Models.ResponseModels.Sia
{
    public class SiaHttpResponse
    {
        public HttpStatusCode Code { get; set; }
        
        public string Message { get; set; }
        
        public IDictionary<string,string> RequestData { get; set; }
    }
}