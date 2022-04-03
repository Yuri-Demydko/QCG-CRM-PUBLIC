using CRM.DAL.Models.DatabaseModels.Configs;
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
using CRM.DAL.Models.DatabaseModels.Users.VerifyCodes;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRM.DAL
{
    public class GeneralDbContext : IdentityDbContext<
        User, Role, string,
        UserClaim, UserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>, IDataProtectionKeyContext
    {
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
        
        public DbSet<Configuration> Configurations { get; set; }

        public DbSet<VerifyCode> VerifyCodes { get; set; }
        public DbSet<EmailVerifyCode> EmailVerifyCodes { get; set; }

        public DbSet<File> Files { get; set; }
        public DbSet<KontragentInfo> KontragentInfos { get; set; }
        public DbSet<Kontragent> Kontragents { get; set; }
        public DbSet<KontragentUser> KontragentUsers { get; set; }
        public DbSet<PayCard> PayCards { get; set; }
        public DbSet<ProductFile> ProductFiles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductKontragent> ProductKontragents { get; set; }
        public DbSet<ProductUser> ProductUsers { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }
        public DbSet<Tag> Tags { get; set; }



        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     base.OnConfiguring(optionsBuilder);
        // }
        //FOR MIGRATIONS
        public GeneralDbContext()
        {

        }
        public GeneralDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql("Host=db-crm-dev-v1.cvt8etkjrnau.eu-central-1.rds.amazonaws.com;Port=5432;Database=postgres;Username=crm;Password=crm123321;CommandTimeout=1000");
        }

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