using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using CRM.DAL.Models.DatabaseModels.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.DAL.Models.DatabaseModels.EmailChanges
{
    [Table("EmailChange")]
    public class EmailChange
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        
        public string UserId { get; set; }
        
        public string NewEmail { get; set; }
        
        public bool Confirmed { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public string UserToken { get; set; }
    }
    
    public class EmailChangeConfiguration : IEntityTypeConfiguration<EmailChange>
    {
        public void Configure(EntityTypeBuilder<EmailChange> item)
        {
            item.HasOne(i => i.User)
                .WithMany(r => r.EmailChanges)
                .HasForeignKey(i => i.UserId);
            item.Property(i => i.Confirmed)
                .HasDefaultValue(false);
        }
    }
}