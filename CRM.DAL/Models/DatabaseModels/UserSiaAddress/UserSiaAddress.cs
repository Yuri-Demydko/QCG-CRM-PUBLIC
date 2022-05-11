using System;
using System.ComponentModel.DataAnnotations.Schema;
using CRM.DAL.Models.DatabaseModels.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.DAL.Models.DatabaseModels.UserSiaAddress
{
    public class UserSiaAddress
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public string UserId { get; set; }
        
        public User User { get; set; }
        
        public string Address { get; set; }
    }
    
    public class UserSiaAddressConfiguration : IEntityTypeConfiguration<UserSiaAddress>
    {
        public void Configure(EntityTypeBuilder<UserSiaAddress> item)
        {
            item.HasOne(i => i.User)
                .WithMany(i => i.SiaAddresses)
                .HasForeignKey(i => i.UserId);
        }
    }
}