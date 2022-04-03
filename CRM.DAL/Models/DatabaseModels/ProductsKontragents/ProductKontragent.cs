using System;
using System.ComponentModel.DataAnnotations.Schema;
using CRM.DAL.Models.DatabaseModels.Kontragents;
using CRM.DAL.Models.DatabaseModels.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.DAL.Models.DatabaseModels.ProductsKontragents
{
    public class ProductKontragent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public Guid ProductId { get; set; }
        
        public Product Product { get; set; }
        
        public Guid KontragentId { get; set; }
        
        public Kontragent Kontragent { get; set; }
        
        public RelationType RelationType { get; set; }
    }
    public class ProductKontragentConfiguration : IEntityTypeConfiguration<ProductKontragent>
    {
        public void Configure(EntityTypeBuilder<ProductKontragent> item)
        {
            item.HasOne(i => i.Product)
                .WithMany(r => r.ProductKontragents)
                .HasForeignKey(i => i.ProductId);
            
            item.HasOne(i => i.Kontragent)
                .WithMany(r => r.ProductKontragents)
                .HasForeignKey(i => i.KontragentId);
        }
    }
}