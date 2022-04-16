using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using CRM.DAL.Models.DatabaseModels.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.DAL.Models.DatabaseModels.PayCards
{
    public class PayCard
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public string UserId { get; set; }
        
        public User User { get; set; }
        
        [MaxLength(16)]//??
        [MinLength(16)]
        public string Number { get; set; }
        
        [MaxLength(3)]
        [MinLength(3)]
        public string CVV { get; set; }
        
        public DateTime ValidTill { get; set; }//@TODO Maybe use datetime here
        
        [NotNull]
        public string OwnerName { get; set; }
    }
    
    public class PayCardConfiguration : IEntityTypeConfiguration<PayCard>
    {
        public void Configure(EntityTypeBuilder<PayCard> item)
        {
            item.HasOne(i => i.User)
                .WithMany(r => r.PayCards)
                .HasForeignKey(i => i.UserId);
            
        }
    }
}