using System;
using System.ComponentModel.DataAnnotations.Schema;
using CRM.DAL.Models.DatabaseModels.Products;

namespace CRM.DAL.Models.DatabaseModels.ProductRequirements
{
    public class ProductRequirements
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public Product Product { get; set; }
        
        public Guid ProductId { get; set; }
        
        public string RequirementKey { get; set; }
        
        public string RequirementValue { get; set; }
    }
}