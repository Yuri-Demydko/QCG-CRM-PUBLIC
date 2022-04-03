using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.DAL.Models.DatabaseModels.Users
{
    public class UserClaim : IdentityUserClaim<string>
    {
        public User User { get; set; }
    }
    public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> item)
        {
            item.HasBaseType((Type) null);

            item.HasOne(i => i.User).WithMany(r => r.UserClaims).HasForeignKey(r => r.UserId);
        }
    }
}