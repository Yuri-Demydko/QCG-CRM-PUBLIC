using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.DAL.Models.Users
{
    public class Role : IdentityRole<string>
    {
        public ICollection<DAL.Models.Users.UserRole> UserRoles { get; set; }
    }


    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> item)
        {
            item.HasMany(i => i.UserRoles).WithOne(i => i.Role).HasForeignKey(r=>r.RoleId);
            
        }
    }
}