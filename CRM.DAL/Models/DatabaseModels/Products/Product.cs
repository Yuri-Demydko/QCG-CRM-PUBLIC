using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CRM.DAL.Models.DatabaseModels.ProductsUsers;
using CRM.DAL.Models.DatabaseModels.Tags;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.DAL.Models.DatabaseModels.Products
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Title { get; set; }
        
        public string FullDescription { get; set; }
        
        public string ShortDescription { get; set; }
        
        public decimal Price { get; set; }
        
        public decimal? DiscountPrice { get; set; }
        
        public DateTime AddedAt { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public int Priority { get; set; }
        
        public ICollection<ProductRequirements.ProductRequirements> Requirements { get; set; }

        public ICollection<Tag> Tags { get; set; }
        
        
        public ICollection<ProductFile.ProductFile> ProductFiles { get; set; }
        
        public ICollection<ProductUser> ProductUsers { get; set; }
        
        public ICollection<ProductsKontragents.ProductKontragent> ProductKontragents { get; set; }
        
        public ICollection<ProductsComments.ProductComment> ProductComments { get; set; }

    }
    
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> item)
        {
            item.HasMany(i => i.ProductFiles)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId);

            item.HasMany(i => i.Tags)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId);
            
            item.HasMany(i => i.ProductUsers)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId);

            item.HasMany(i => i.Requirements)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId);
            
            item.HasMany(i => i.ProductKontragents)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId);

            item.HasMany(i => i.ProductComments)
                .WithOne(p => p.Product)
                .HasForeignKey(i => i.ProductId);

            item.Property(i => i.Priority)
                .HasDefaultValue(0);

            item.Property(i => i.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            item.Property(i => i.AddedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}