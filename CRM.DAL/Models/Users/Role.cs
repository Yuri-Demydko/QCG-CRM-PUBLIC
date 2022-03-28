using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.DAL.Models.Users
{
    public class Role : IdentityRole<string>
    {
        protected internal ICollection<UserRole> UserRoles { get; set; }
    }


    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> item)
        {
            item.HasMany(i => i.UserRoles).WithOne(i => i.Role);

            // @TODO придумать нормальный data seed
            // foreach (var roleName in UserRoles.RoleNames.Keys)
            // {
            //     item.HasData(new List<Role>()
            //     {
            //         new Role() {Name = roleName, NormalizedName = roleName.ToUpper(), Id = Guid.NewGuid().ToString()}
            //     });
            // }
        }
    }
}