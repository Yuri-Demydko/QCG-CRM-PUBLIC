﻿using System.Collections.Generic;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Mvc;

namespace CRM.User.WebApp.Models.Basic.Product
{
    /// <summary>
    ///     Represents the model configuration for User.
    /// </summary>
    public class ProductOdataConfiguration : IModelConfiguration
    {
        /// <summary>
        ///     Applies model configurations using the provided builder for the specified API version.
        /// </summary>
        /// <param name="builder">The <see cref="ODataModelBuilder">builder</see> used to apply configurations.</param>
        /// <param name="apiVersion">The <see cref="ApiVersion">API version</see> associated with the <paramref name="builder" />.</param>
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion)
        {
            var item = builder.EntitySet<DAL.Models.Products.Product>(nameof(DAL.Models.Products.Product)).EntityType;

            // item.Ignore(i => i.UserName);
            // item.Ignore(i => i.NormalizedUserName);
            //
            // item.Ignore(i => i.EmailConfirmed);
            // item.Ignore(i => i.NormalizedEmail);
            //
            // item.Ignore(i => i.SecurityStamp);
            // item.Ignore(i => i.ConcurrencyStamp);
            //
            // item.Ignore(i => i.PasswordHash);
            //
            // item.Ignore(i => i.LockoutEnabled);
            // item.Ignore(i => i.LockoutEnd);
            //
            // item.Ignore(i => i.AccessFailedCount);
            //
            // item.Ignore(i => i.PhoneNumberConfirmed);
            //
            // item.Ignore(i => i.TwoFactorEnabled);
            //
            // item.Ignore(i => i.UserRoles);
            //
            // item.Collection.Function("Profile")
            //     .ReturnsFromEntitySet<User.UserProfileDto.UserProfileDto>(nameof(User.UserProfileDto));
            // item.Collection.Function("Policies").ReturnsCollection<IEnumerable<string>>();
            // item.Collection.Function("Roles").ReturnsCollection<IEnumerable<string>>();
            //
            //
            // item.Collection.Action("ChangePassword");
            //
            item.Action("AddToFavorites");
            item.Action("RemoveFromFavorites");
            item.HasKey(p => p.Id);
        }
    }
}