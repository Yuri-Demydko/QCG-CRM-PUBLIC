using System.Collections.Generic;

namespace CRM.DAL.Models.RequestModels.ProductBuy
{
    public class ProductBuyRequestResult
    {
        public bool IsSuccess { get; set; }
        
        public List<ProductBuyErrorCode> ErrorCodes { get; set; }
    }
}