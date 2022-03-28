using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CRM.DAL.Models.Files;
using CRM.DAL.Models.Genres;
using CRM.DAL.Models.ProductsUsers;
using CRM.DAL.Models.Tags;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.DAL.Models.Products
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Title { get; set; }
        
        public string FullDescription { get; set; }
        
        public string ShortDescription { get; set; }
        
        public ICollection<ProductRequirements> Requirements { get; set; }//
        
        // public File Icon { get; set; }
        //
        // public Guid IconId { get; set; }
        //
        // public File Cover { get; set; }
        // public Guid CoverId { get; set; }
        
        public ICollection<Tag> Tags { get; set; }
        
        
        public ICollection<ProductFile.ProductFile> ProductFiles { get; set; }
        
        public ICollection<ProductUser> ProductUsers { get; set; }
        
        public ICollection<ProductsKontragents.ProductKontragent> ProductKontragents { get; set; }
        
        public Genre Genre { get; set; }


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

            item.HasOne(r => r.Genre);

            item.HasMany(i => i.Requirements)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId);
            
            item.HasMany(i => i.ProductKontragents)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId);
        }
    }
}