using System.Collections.Generic;
using System.Threading.Tasks;
using CRM.DAL.Models.DatabaseModels.PayCards;

namespace CRM.User.WebApp.Services.PayCardValidationService
{
    public class PayCardValidationServiceMock:IPayCardValidationService
    {
        public async Task<PayCardValidationResult> ValidateAsync(PayCard item) =>
            new PayCardValidationResult
            {
                IsSuccess = true,
                Errors = new List<string>()
            };
    }
}