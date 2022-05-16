using System;
using System.Collections.Generic;
using CRM.DAL.Models.DatabaseModels.Products;
using CRM.DAL.Models.RequestModels.ProductBuy.Enums;

namespace CRM.DAL.Models.RequestModels.ProductBuy
{
    public class ProductBuyRequest
    {
        public int Id { get; set; }//for odata only

        public string UserId { get; set; }
        
        public List<Guid> ProductIds { get; set; }
        
        public Guid? PayCardId { get; set; }
        
        public ProductBuyRequestPaymentType PaymentType { get; set; }
    }
}