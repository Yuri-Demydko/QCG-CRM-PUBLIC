﻿using System;
using System.Collections.Generic;
using CRM.DAL.Models.KontragentUsers;
using CRM.DAL.Models.PayCards;
using CRM.DAL.Models.ProductsUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.DAL.Models.Users
{
    public class User : IdentityUser
    {

        public ICollection<UserRole> UserRoles { get; set; }
        
        public ICollection<UserClaim> UserClaims { get; set; }

        public bool IsActive { get; set; }
        
        public ICollection<PayCard> PayCards { get; set; }
        
        public ICollection<ProductUser> ProductUsers { get; set; }
        
        public ICollection<KontragentUser> KontragentUsers { get; set; }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> item)
        {
            item.HasBaseType((Type) null);
            
            item.HasMany(i => i.UserRoles).WithOne(i => i.User).HasForeignKey(i=>i.UserId);
            
            item.HasMany(i => i.UserClaims).WithOne(i => i.User).HasForeignKey(i=>i.UserId);

            item.HasMany(i => i.PayCards).WithOne(i => i.User).HasForeignKey(i => i.UserId);
            item.HasMany(i => i.ProductUsers).WithOne(i => i.User).HasForeignKey(i => i.UserId);
        }
    }
}