using System.Collections.Generic;
using System.Threading.Tasks;
using CRM.DAL.Models.RequestModels.ProductBuy;
using CRM.User.WebApp.Models.Basic;

namespace CRM.User.WebApp.Services.ProductBuy
{
    public class ProductBuyServiceMock:IProductBuyService
    {
        private readonly UserDbContext dbContext;

        public ProductBuyServiceMock(UserDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ProductBuyRequestResult> ProcessRequestAsync(ProductBuyRequest request)
        {
            return new ProductBuyRequestResult()
            {
                IsSuccess = true,
                ErrorCodes = new List<ProductBuyErrorCode>()
            };
        }
    }
}