using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Mvc;

namespace CRM.User.WebApp.Models.Basic.PayCard
{
    /// <summary>
    ///     Represents the model configuration for User.
    /// </summary>
    public class PayCardOdataConfiguration : IModelConfiguration
    {
        /// <summary>
        ///     Applies model configurations using the provided builder for the specified API version.
        /// </summary>
        /// <param name="builder">The <see cref="ODataModelBuilder">builder</see> used to apply configurations.</param>
        /// <param name="apiVersion">The <see cref="ApiVersion">API version</see> associated with the <paramref name="builder" />.</param>
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion)
        {
            var item = builder.EntitySet<DAL.Models.DatabaseModels.PayCards.PayCard>(nameof(DAL.Models.DatabaseModels.PayCards.PayCard)).EntityType;
            
            item.HasKey(p => p.Id);
        }
    }
}