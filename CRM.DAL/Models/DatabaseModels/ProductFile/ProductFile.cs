using System;
using System.ComponentModel.DataAnnotations.Schema;
using CRM.DAL.Models.DatabaseModels.Files;
using CRM.DAL.Models.DatabaseModels.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.DAL.Models.DatabaseModels.ProductFile
{
    public class ProductFile
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public Guid ProductId { get; set; }
        
        public Guid FileId { get; set; }
        
        public File File { get; set; }
        
        public Product Product { get; set; }

        public ProductFileType ProductFileType { get; set; }
    }
    
    public class ProductFileConfiguration : IEntityTypeConfiguration<ProductFile>
    {
        public void Configure(EntityTypeBuilder<ProductFile> item)
        {
            item.HasOne(i => i.Product)
                .WithMany(r => r.ProductFiles)
                .HasForeignKey(i => i.ProductId);
            
        }
    }
}