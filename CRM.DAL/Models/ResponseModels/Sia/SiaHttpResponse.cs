using System.Net;

namespace CRM.DAL.Models.ResponseModels.Sia
{
    public class SiaHttpResponse
    {
        public HttpStatusCode Code { get; set; }
        
        public string Message { get; set; }
    }
}