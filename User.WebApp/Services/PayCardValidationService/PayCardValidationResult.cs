using System.Collections.Generic;

namespace CRM.User.WebApp.Services.PayCardValidationService
{
    public class PayCardValidationResult
    {
        public bool IsSuccess { get; set; }
        public IList<string> Errors { get; set; }
    }
}