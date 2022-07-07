using System.ComponentModel.DataAnnotations;

namespace CRM.DAL.Models.RequestModels.Shop
{
    public class ShopRequest 
    {     
        public int Id { get; set; }//for odata only
        public int? CurrentPage { get; set; }
    }
}