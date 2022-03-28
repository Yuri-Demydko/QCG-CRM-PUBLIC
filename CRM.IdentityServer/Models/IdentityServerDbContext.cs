using CRM.DAL.Models;
using CRM.DAL.Models.Files;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRM.IdentityServer.Models
{
    public class IdentityServerDbContext : IdentityDbContext<User.User>, IDataProtectionKeyContext
    {
        public IdentityServerDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        public DbSet<File> Files { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<File>().HasQueryFilter(i => !i.IsDeleted);

        }
    }
}