using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.DAL.Models.DatabaseModels.Users
{
    public class UserRole : IdentityUserRole<string>
    {
        public User User { get; set; }
        
        public Role Role { get; set; }
        
    }
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> item)
        {
            item.HasBaseType((Type) null);

            item.HasOne(i => i.User).WithMany(r => r.UserRoles).HasForeignKey(r => r.UserId);
            item.HasOne(i => i.Role).WithMany(r => r.UserRoles).HasForeignKey(r => r.RoleId);
        }
    }
    
    
}