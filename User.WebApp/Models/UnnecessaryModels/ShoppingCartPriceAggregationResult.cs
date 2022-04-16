using System.ComponentModel.DataAnnotations;

namespace CRM.User.WebApp.Models.UnnecessaryModels
{
    public class ShoppingCartPriceAggregationResult
    {
        //Necessary property for OData
        [Key]
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalRawPrice { get; set; }
        public decimal TotalDiscount { get; set; }
    }
}