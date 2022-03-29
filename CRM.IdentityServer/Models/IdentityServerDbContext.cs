using CRM.DAL.Models.Users;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRM.IdentityServer.Models
{
    public class IdentityServerDbContext : IdentityDbContext<User, Role, string,
        UserClaim, UserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>,
        IdentityUserToken<string>>, IDataProtectionKeyContext
    {
        public IdentityServerDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
            public DbSet<User> Users { get; set; }
            public DbSet<Role> Roles { get; set; }
            public DbSet<UserRole> UserRoles { get; set; }
            public DbSet<UserClaim> UserClaims { get; set; }
     //   public DbSet<File> Files { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            //modelBuilder.Entity<File>().HasQueryFilter(i => !i.IsDeleted);

        }
    }
}