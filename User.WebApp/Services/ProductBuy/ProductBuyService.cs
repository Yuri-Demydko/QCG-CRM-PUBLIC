using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.DAL.Models.RequestModels.ProductBuy;
using CRM.DAL.Models.RequestModels.ProductBuy.Enums;
using CRM.ServiceCommon.Clients;
using CRM.User.WebApp.Models.Basic;
using Microsoft.Extensions.Logging;

namespace CRM.User.WebApp.Services.ProductBuy
{
    public class ProductBuyService:IProductBuyService
    {
        private readonly UserDbContext userDbContext;
        private readonly ISiaPriceClient siaPriceClient;
        private readonly ILogger<ProductBuyService> logger;

        public ProductBuyService(UserDbContext userDbContext, ISiaPriceClient siaPriceClient, ILogger<ProductBuyService> logger)
        {
            this.userDbContext = userDbContext;
            this.siaPriceClient = siaPriceClient;
            this.logger = logger;
        }

        public async Task<ProductBuyRequestResult> ProcessRequestAsync(ProductBuyRequest request)
        {
            var totalPrice = userDbContext.Products
                .Where(r => request.ProductIds.Contains(r.Id))
                .Sum(r => r.DiscountPrice ?? r.Price);

            switch (request.PaymentType)
            {
                case ProductBuyRequestPaymentType.Sia:
                {
                    var user = userDbContext.Users.FirstOrDefault(r => r.Id == request.UserId);
                    var siaPrice = await siaPriceClient.GetSiaPriceAsync();
                    var totalSiaPrice = totalPrice * siaPrice;

                    if (user == null)
                    {
                        logger.LogWarning($"product buy request service - user not found (id: {request.UserId})");
                        return new ProductBuyRequestResult()
                        {
                            IsSuccess = false,
                            ErrorCodes = new List<ProductBuyErrorCode> {ProductBuyErrorCode.UserNotFound}
                        };
                    }
                    
                    if (totalSiaPrice > user.SiaCoinBalance)
                    {
                        logger.LogWarning($"product buy request service - user {user.UserName} not enough sia balance ({user.SiaCoinBalance})");
                        return new ProductBuyRequestResult()
                        {
                            IsSuccess = false,
                            ErrorCodes = new List<ProductBuyErrorCode> {ProductBuyErrorCode.NotEnoughBalance}
                        };
                    }

                    user.SiaCoinBalance -= totalSiaPrice;
                    await userDbContext.SaveChangesAsync();

                    return new ProductBuyRequestResult
                    {
                        IsSuccess = true
                    };
                }
                case ProductBuyRequestPaymentType.QRON:
                {
                    throw new NotImplementedException("QRON pay not implemented now");
                }
            }

            throw new ArgumentException("Wrong argument");
        }
    }
}