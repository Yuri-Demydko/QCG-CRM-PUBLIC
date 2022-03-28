using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM.DAL.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.DAL.Models.PayCards
{
    public class PayCard
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public string UserId { get; set; }
        
        public User User { get; set; }
        
        [MaxLength(16)]//??
        public string Number { get; set; }
        
        [MaxLength(3)]
        public string CVV { get; set; }
        
        [MaxLength(6)]
        public string ValidTill { get; set; }//@TODO Maybe use datetime here
        
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