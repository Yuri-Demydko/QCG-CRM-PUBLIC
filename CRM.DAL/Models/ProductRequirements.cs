using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.DAL.Models
{
    public class ProductRequirements
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public Products.Product Product { get; set; }
        
        public Guid ProductId { get; set; }
        
        public string RequirementKey { get; set; }
        
        public string RequirementValue { get; set; }
    }
}