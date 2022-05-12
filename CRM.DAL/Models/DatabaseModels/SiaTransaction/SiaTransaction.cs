using System;
using System.ComponentModel.DataAnnotations.Schema;
using CRM.DAL.Models.DatabaseModels.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.DAL.Models.DatabaseModels.SiaTransaction
{
    [Table("SiaTransactions")]
    public class SiaTransaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public string SiaId { get; set; }
        
        public decimal CoinsValue { get; set; }
        
        public long InitialHeight { get; set; }
        
        public string UserId { get; set; }
        
        public User User { get; set; }
        
        public long Confirmations { get; set; }
        
        public DateTime RegistrationTime { get; set; }
        
        public string DestinationAddress { get; set; }
        
        public bool OnBalance { get; set; }

    }
    
    public class SiaTransactionConfiguration : IEntityTypeConfiguration<SiaTransaction>
    {
        public void Configure(EntityTypeBuilder<SiaTransaction> item)
        {
            item.HasOne(i => i.User)
                .WithMany(i => i.SiaTransactions)
                .HasForeignKey(i => i.UserId);
        }
    }
}