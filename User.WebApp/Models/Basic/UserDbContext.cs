using CRM.DAL.Models;
using CRM.DAL.Models.Files;
using CRM.User.WebApp.Models.Basic.Role;
using CRM.User.WebApp.Models.Basic.User;
using CRM.User.WebApp.Models.Basic.UserRole;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRM.User.WebApp.Models.Basic
{
    public class UserDbContext : IdentityDbContext<
        User.User, Role.Role, string,
        IdentityUserClaim<string>, UserRole.UserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>, IDataProtectionKeyContext
    {
        public string UserId { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
        

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

       

        public DbSet<File> Files { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfiguration(new UserContextConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleContextConfiguration());
            modelBuilder.ApplyConfiguration(new RoleContextConfiguration());
           
        }
    }
}