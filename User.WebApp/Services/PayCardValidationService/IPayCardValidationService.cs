using System.Threading.Tasks;
using CRM.DAL.Models.DatabaseModels.PayCards;

namespace CRM.User.WebApp.Services.PayCardValidationService
{
    public interface IPayCardValidationService
    {
        public  Task<PayCardValidationResult> ValidateAsync(PayCard item);
    }
}