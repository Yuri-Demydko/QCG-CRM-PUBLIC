using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CRM.User.WebApp.Models.Basic.Shop
{
    /// <summary>
    ///     Represents the model configuration for User.
    /// </summary>
    public class ShopViewModelOdataConfiguration : IModelConfiguration
    {
        /// <summary>
        ///     Applies model configurations using the provided builder for the specified API version.
        /// </summary>
        /// <param name="builder">The <see cref="ODataModelBuilder">builder</see> used to apply configurations.</param>
        /// <param name="apiVersion">The <see cref="ApiVersion">API version</see> associated with the <paramref name="builder" />.</param>
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion)
        {

            var item = builder.EntitySet<CRM.User.WebApp.Models.ViewModel.Shop.Shop>(nameof(CRM.User.WebApp.Models.ViewModel.Shop.Shop)).EntityType;

            item.HasKey(i => i.Id);

            item.Action("ShopList");
        }
    }
}