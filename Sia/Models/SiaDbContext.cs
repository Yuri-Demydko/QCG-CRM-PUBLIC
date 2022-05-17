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
using CRM.DAL.Models.DatabaseModels.SiaMonitoredBlock;
using CRM.DAL.Models.DatabaseModels.SiaRenterAllowances;
using CRM.DAL.Models.DatabaseModels.SiaTransaction;
using CRM.DAL.Models.DatabaseModels.Tags;
using CRM.DAL.Models.DatabaseModels.Users;
using CRM.DAL.Models.DatabaseModels.UserSiaAddress;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Sia.Models
{
    public class SiaDbContext : IdentityDbContext<
        User, Role, string,
        UserClaim, UserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>, IDataProtectionKeyContext
    {
        public string UserId { get; set; }
        

        public SiaDbContext(DbContextOptions<SiaDbContext> options) : base(options)
        {
        }
        

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        public DbSet<EmailChange> EmailChanges { get; set; }

        public DbSet<File> Files { get; set; }
        
        public DbSet<KontragentInfo> KontragentInfos { get; set; }
        
        public DbSet<Kontragent> Kontragents { get; set; }
        
        public DbSet<KontragentUser> KontragentUsers { get; set; }
        
        public DbSet<PayCard> PayCards { get; set; }
        
        public DbSet<ProductFile> ProductFiles { get; set; }
        
        public DbSet<Product> Products { get; set; }
        
        public DbSet<ProductKontragent> ProductKontragents { get; set; }
        
        public DbSet<ProductUser> ProductUsers { get; set; }
        
        public DbSet<Tag> Tags { get; set; }
        
        public DbSet<ProductComment> ProductComments { get; set; }
        
        public DbSet<SiaMonitoredBlock> SiaMonitoredBlocks { get; set; }
        
        public DbSet<SiaTransaction> SiaTransactions { get; set; }
        
        public DbSet<UserSiaAddress> UserSiaAddresses { get; set; }
        
        public DbSet<SiaRenterAllowance> SiaRenterAllowances { get; set; }



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

            modelBuilder.ApplyConfiguration(new SiaTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new UserSiaAddressConfiguration());

        }
    }
}