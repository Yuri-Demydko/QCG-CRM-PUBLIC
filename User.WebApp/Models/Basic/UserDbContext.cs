using CRM.DAL.Models.DatabaseModels.EmailChanges;
using CRM.DAL.Models.DatabaseModels.Files;
using CRM.DAL.Models.DatabaseModels.KontragentInfo;
using CRM.DAL.Models.DatabaseModels.Kontragents;
using CRM.DAL.Models.DatabaseModels.KontragentUsers;
using CRM.DAL.Models.DatabaseModels.PayCards;
using CRM.DAL.Models.DatabaseModels.ProductFile;
using CRM.DAL.Models.DatabaseModels.Products;
using CRM.DAL.Models.DatabaseModels.ProductsComments;
using CRM.DAL.Models.DatabaseModels.ProductsKontragents;
using CRM.DAL.Models.DatabaseModels.ProductsUsers;
using CRM.DAL.Models.DatabaseModels.Tags;
using CRM.DAL.Models.DatabaseModels.Users;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRM.User.WebApp.Models.Basic
{
    public class UserDbContext : IdentityDbContext<
        DAL.Models.DatabaseModels.Users.User, DAL.Models.DatabaseModels.Users.Role, string,
        UserClaim, UserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>, IDataProtectionKeyContext
    {
        public string UserId { get; set; }
        

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
        

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        public DbSet<EmailChange> EmailChanges { get; set; }

        public DbSet<File> Files { get; set; }
        
        public DbSet<KontragentInfo> KontragentInfos { get; set; }
        
        public DbSet<Kontragent> Kontragents { get; set; }
        
        public DbSet<KontragentUser> KontragentUsers { get; set; }
        
        public DbSet<DAL.Models.DatabaseModels.PayCards.PayCard> PayCards { get; set; }
        
        public DbSet<DAL.Models.DatabaseModels.ProductFile.ProductFile> ProductFiles { get; set; }
        
        public DbSet<DAL.Models.DatabaseModels.Products.Product> Products { get; set; }
        
        public DbSet<ProductKontragent> ProductKontragents { get; set; }
        
        public DbSet<DAL.Models.DatabaseModels.ProductsUsers.ProductUser> ProductUsers { get; set; }
        
        public DbSet<Tag> Tags { get; set; }
        
        public DbSet<DAL.Models.DatabaseModels.ProductsComments.ProductComment> ProductComments { get; set; }



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