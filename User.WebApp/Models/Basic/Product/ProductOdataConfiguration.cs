using System;
using System.Collections.Generic;
using CRM.DAL.Models.RequestModels.ProductBuy;
using CRM.User.WebApp.Models.UnnecessaryModels;
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
            var item = builder.EntitySet<DAL.Models.DatabaseModels.Products.Product>(nameof(DAL.Models.DatabaseModels.Products.Product)).EntityType;

            
             item.Action("Like");
             item.Action("Dislike");
            item.Action("RemoveReaction");
            item.Action("AddToCart");
            item.Action("RemoveFromCart");
            item.Collection.Action("ClearCart");
            item.Collection.Function("CartTotalPrices")
                .ReturnsFromEntitySet<ShoppingCartPriceAggregationResult>(nameof(ShoppingCartPriceAggregationResult));
            item.Property(i => i.Price).OrderBy();
            item.Property(i => i.DiscountPrice).OrderBy();
            item.Property(i => i.CreatedAt).OrderBy();
            item.Property(i => i.AddedAt).OrderBy();
            item.Property(i => i.Priority).OrderBy();
            item.OrderBy();
            item.HasKey(p => p.Id);
        }
    }
}