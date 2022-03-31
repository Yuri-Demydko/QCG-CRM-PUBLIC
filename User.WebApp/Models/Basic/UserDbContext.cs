using CRM.DAL.Models.Files;
using CRM.DAL.Models.KontragentInfo;
using CRM.DAL.Models.Kontragents;
using CRM.DAL.Models.KontragentUsers;
using CRM.DAL.Models.PayCards;
using CRM.DAL.Models.ProductFile;
using CRM.DAL.Models.Products;
using CRM.DAL.Models.ProductsComments;
using CRM.DAL.Models.ProductsKontragents;
using CRM.DAL.Models.ProductsUsers;
using CRM.DAL.Models.Tags;
using CRM.DAL.Models.Users;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRM.User.WebApp.Models.Basic
{
    public class UserDbContext : IdentityDbContext<
        DAL.Models.Users.User, DAL.Models.Users.Role, string,
        UserClaim, UserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>, IDataProtectionKeyContext
    {
        public string UserId { get; set; }
        

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
        

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }


        public DbSet<File> Files { get; set; }
        
        public DbSet<KontragentInfo> KontragentInfos { get; set; }
        
        public DbSet<Kontragent> Kontragents { get; set; }
        
        public DbSet<KontragentUser> KontragentUsers { get; set; }
        
        public DbSet<PayCard> PayCards { get; set; }
        
        public DbSet<DAL.Models.ProductFile.ProductFile> ProductFiles { get; set; }
        
        public DbSet<DAL.Models.Products.Product> Products { get; set; }
        
        public DbSet<ProductKontragent> ProductKontragents { get; set; }
        
        public DbSet<ProductUser> ProductUsers { get; set; }
        
        public DbSet<Tag> Tags { get; set; }
        
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserClaimConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            
            modelBuilder.ApplyConfiguration(new TagConfiguration());
            modelBuilder.ApplyConfiguration(new ProductUserConfiguration());
            modelBuilder.ApplyConfiguration(new ProductKontragentConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductFileConfiguration());
            modelBuilder.ApplyConfiguration(new PayCardConfiguration());
            modelBuilder.ApplyConfiguration(new KontragentUserConfiguration());
            modelBuilder.ApplyConfiguration(new KontragentConfiguration());
            modelBuilder.ApplyConfiguration(new KontragentInfoConfiguration());
            modelBuilder.ApplyConfiguration(new FileConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCommentConfiguration());

        }
    }
}