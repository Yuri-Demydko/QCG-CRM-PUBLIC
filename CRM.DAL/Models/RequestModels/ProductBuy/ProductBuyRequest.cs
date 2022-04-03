using System;
using System.Collections.Generic;
using CRM.DAL.Models.DatabaseModels.Products;

namespace CRM.DAL.Models.RequestModels.ProductBuy
{
    public class ProductBuyRequest
    {
        public int Id { get; set; }
        
        // public List<Product> Products { get; set; }
        
        public string UserId { get; set; }
        
        public List<Guid> ProductIds { get; set; }
        
        public Guid? PayCardId { get; set; }
    }
}