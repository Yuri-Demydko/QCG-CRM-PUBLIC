using System.Threading.Tasks;
using CRM.DAL.Models.RequestModels.ProductBuy;

namespace CRM.User.WebApp.Services
{
    public interface IProductBuyService
    {
        public Task<ProductBuyRequestResult> ProcessRequestAsync(ProductBuyRequest request);
    }
}