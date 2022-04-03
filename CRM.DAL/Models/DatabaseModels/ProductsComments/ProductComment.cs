using System;
using System.ComponentModel.DataAnnotations.Schema;
using CRM.DAL.Models.DatabaseModels.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.DAL.Models.DatabaseModels.ProductsComments
{
    public class ProductComment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public Users.User User { get; set; }
        
        public string UserId { get; set; }
        
        public Product Product { get; set; }
        
        public Guid ProductId { get; set; }

        public string Comment { get; set; }
        
    }
    public class ProductCommentConfiguration : IEntityTypeConfiguration<ProductComment>
    {
        public void Configure(EntityTypeBuilder<ProductComment> item)
        {
            item.HasBaseType((Type) null);

            item.HasOne(i => i.User)
                .WithMany(i => i.ProductComments)
                .HasForeignKey(i => i.UserId);
            
            item.HasOne(i => i.Product)
                .WithMany(i => i.ProductComments)
                .HasForeignKey(i => i.ProductId);
        }
    }
}