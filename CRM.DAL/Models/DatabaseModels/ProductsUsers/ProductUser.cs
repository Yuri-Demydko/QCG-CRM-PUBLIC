using System;
using System.ComponentModel.DataAnnotations.Schema;
using CRM.DAL.Models.DatabaseModels.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.DAL.Models.DatabaseModels.ProductsUsers
{
    public class ProductUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        
        public Guid ProductId { get; set; }

        public Users.User User { get; set; }
        
        public Product Product { get; set; }
        
        public ProductUserRelationType RelationType { get; set; }
    }
    public class ProductUserConfiguration : IEntityTypeConfiguration<ProductUser>
    {
        public void Configure(EntityTypeBuilder<ProductUser> item)
        {
            item.HasOne(i => i.Product)
                .WithMany(r => r.ProductUsers)
                .HasForeignKey(i => i.ProductId);
            
            item.HasOne(i => i.User)
                .WithMany(r => r.ProductUsers)
                .HasForeignKey(i => i.UserId);
        }
    }
}