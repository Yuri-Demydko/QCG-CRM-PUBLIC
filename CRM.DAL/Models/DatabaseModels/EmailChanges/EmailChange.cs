using System;
using System.ComponentModel;
using CRM.DAL.Models.DatabaseModels.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.DAL.Models.DatabaseModels.EmailChanges
{
    public class EmailChange
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        
        public string UserId { get; set; }
        
        public string NewEmail { get; set; }
        
        public bool Confirmed { get; set; }
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