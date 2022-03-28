using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.IdentityServer.Models.User
{
    public class UserContextConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> item)
        {
            item.HasBaseType((Type)null);
        }
    }
}