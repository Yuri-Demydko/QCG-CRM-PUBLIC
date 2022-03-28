using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.User.WebApp.Models.Basic.UserRole
{
    public class UserRoleContextConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> item)
        {
            item.HasBaseType((Type)null);
        }
    }
}