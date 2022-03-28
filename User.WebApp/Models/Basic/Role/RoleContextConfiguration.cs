using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.User.WebApp.Models.Basic.Role
{
    public class RoleContextConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> item)
        {
            item.HasBaseType((Type)null);

            item.HasMany(e => e.UserRoles)
                .WithOne(x => x.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        }
    }
}