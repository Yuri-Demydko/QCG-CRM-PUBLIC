// <auto-generated />
using System;
using CRM.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CRM.DAL.Migrations
{
    [DbContext(typeof(GeneralDbContext))]
    [Migration("20220405061437_updateProducts")]
    partial class updateProducts
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.15")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.Configs.Configuration", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CommonConfigs");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.Files.File", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ContentType")
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("OriginalName")
                        .HasColumnType("text");

                    b.Property<string>("Path")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.KontragentInfo.KontragentInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("InfoKey")
                        .HasColumnType("text");

                    b.Property<string>("InfoValue")
                        .HasColumnType("text");

                    b.Property<Guid>("KontragentId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("KontragentId");

                    b.ToTable("KontragentInfos");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.KontragentUsers.KontragentUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("KontragentId")
                        .HasColumnType("uuid");

                    b.Property<int>("RelationType")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("KontragentId");

                    b.HasIndex("UserId");

                    b.ToTable("KontragentUsers");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.Kontragents.Kontragent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("IconId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("IconId");

                    b.ToTable("Kontragents");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.PayCards.PayCard", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CVV")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<string>("Number")
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.Property<string>("OwnerName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("ValidTill")
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("PayCards");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.ProductFile.ProductFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("FileId")
                        .HasColumnType("uuid");

                    b.Property<int>("ProductFileType")
                        .HasColumnType("integer");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductFiles");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.ProductRequirements.ProductRequirements", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<string>("RequirementKey")
                        .HasColumnType("text");

                    b.Property<string>("RequirementValue")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductRequirements");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.Products.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("AddedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<decimal?>("DiscountPrice")
                        .HasColumnType("numeric");

                    b.Property<string>("FullDescription")
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("Priority")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.Property<string>("ShortDescription")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.ProductsComments.ProductComment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("ProductComments");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.ProductsKontragents.ProductKontragent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("KontragentId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<int>("RelationType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("KontragentId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductKontragents");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.ProductsUsers.ProductUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<int>("RelationType")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("ProductUsers");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.Tags.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<string>("TagText")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.Users.Role", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.Users.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.Users.UserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.Users.UserRole", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.Users.VerifyCodes.VerifyCode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsVerify")
                        .HasColumnType("boolean");

                    b.Property<int>("TryCount")
                        .HasColumnType("integer");

                    b.Property<int>("VerifyCodeType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("VerifyCodes");

                    b.HasDiscriminator<string>("Discriminator").HasValue("VerifyCode");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.DataProtection.EntityFrameworkCore.DataProtectionKey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("FriendlyName")
                        .HasColumnType("text");

                    b.Property<string>("Xml")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("DataProtectionKeys");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.Users.VerifyCodes.EmailVerifyCode", b =>
                {
                    b.HasBaseType("CRM.DAL.Models.DatabaseModels.Users.VerifyCodes.VerifyCode");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.ToTable("VerifyCodes");

                    b.HasDiscriminator().HasValue("EmailVerifyCode");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.KontragentInfo.KontragentInfo", b =>
                {
                    b.HasOne("CRM.DAL.Models.DatabaseModels.Kontragents.Kontragent", "Kontragent")
                        .WithMany("KontragentInfo")
                        .HasForeignKey("KontragentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kontragent");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.KontragentUsers.KontragentUser", b =>
                {
                    b.HasOne("CRM.DAL.Models.DatabaseModels.Kontragents.Kontragent", "Kontragent")
                        .WithMany("KontragentUsers")
                        .HasForeignKey("KontragentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CRM.DAL.Models.DatabaseModels.Users.User", "User")
                        .WithMany("KontragentUsers")
                        .HasForeignKey("UserId");

                    b.Navigation("Kontragent");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.Kontragents.Kontragent", b =>
                {
                    b.HasOne("CRM.DAL.Models.DatabaseModels.Files.File", "Icon")
                        .WithMany()
                        .HasForeignKey("IconId");

                    b.Navigation("Icon");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.PayCards.PayCard", b =>
                {
                    b.HasOne("CRM.DAL.Models.DatabaseModels.Users.User", "User")
                        .WithMany("PayCards")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.ProductFile.ProductFile", b =>
                {
                    b.HasOne("CRM.DAL.Models.DatabaseModels.Files.File", "File")
                        .WithMany("ProductFiles")
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CRM.DAL.Models.DatabaseModels.Products.Product", "Product")
                        .WithMany("ProductFiles")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("File");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.ProductRequirements.ProductRequirements", b =>
                {
                    b.HasOne("CRM.DAL.Models.DatabaseModels.Products.Product", "Product")
                        .WithMany("Requirements")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.ProductsComments.ProductComment", b =>
                {
                    b.HasOne("CRM.DAL.Models.DatabaseModels.Products.Product", "Product")
                        .WithMany("ProductComments")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CRM.DAL.Models.DatabaseModels.Users.User", "User")
                        .WithMany("ProductComments")
                        .HasForeignKey("UserId");

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.ProductsKontragents.ProductKontragent", b =>
                {
                    b.HasOne("CRM.DAL.Models.DatabaseModels.Kontragents.Kontragent", "Kontragent")
                        .WithMany("ProductKontragents")
                        .HasForeignKey("KontragentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CRM.DAL.Models.DatabaseModels.Products.Product", "Product")
                        .WithMany("ProductKontragents")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kontragent");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.ProductsUsers.ProductUser", b =>
                {
                    b.HasOne("CRM.DAL.Models.DatabaseModels.Products.Product", "Product")
                        .WithMany("ProductUsers")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CRM.DAL.Models.DatabaseModels.Users.User", "User")
                        .WithMany("ProductUsers")
                        .HasForeignKey("UserId");

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.Tags.Tag", b =>
                {
                    b.HasOne("CRM.DAL.Models.DatabaseModels.Products.Product", "Product")
                        .WithMany("Tags")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.Users.UserClaim", b =>
                {
                    b.HasOne("CRM.DAL.Models.DatabaseModels.Users.User", "User")
                        .WithMany("UserClaims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.Users.UserRole", b =>
                {
                    b.HasOne("CRM.DAL.Models.DatabaseModels.Users.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CRM.DAL.Models.DatabaseModels.Users.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("CRM.DAL.Models.DatabaseModels.Users.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CRM.DAL.Models.DatabaseModels.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("CRM.DAL.Models.DatabaseModels.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.Files.File", b =>
                {
                    b.Navigation("ProductFiles");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.Kontragents.Kontragent", b =>
                {
                    b.Navigation("KontragentInfo");

                    b.Navigation("KontragentUsers");

                    b.Navigation("ProductKontragents");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.Products.Product", b =>
                {
                    b.Navigation("ProductComments");

                    b.Navigation("ProductFiles");

                    b.Navigation("ProductKontragents");

                    b.Navigation("ProductUsers");

                    b.Navigation("Requirements");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.Users.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("CRM.DAL.Models.DatabaseModels.Users.User", b =>
                {
                    b.Navigation("KontragentUsers");

                    b.Navigation("PayCards");

                    b.Navigation("ProductComments");

                    b.Navigation("ProductUsers");

                    b.Navigation("UserClaims");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
