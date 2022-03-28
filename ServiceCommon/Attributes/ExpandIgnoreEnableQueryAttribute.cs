using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;

namespace CRM.ServiceCommon.Attributes
{
    public class ExpandIgnoreEnableQueryAttribute : EnableQueryAttribute
    {
        public override IQueryable ApplyQuery(IQueryable queryable, ODataQueryOptions queryOptions)
        {
            return queryOptions.ApplyTo(queryable, AllowedQueryOptions.Expand | AllowedQueryOptions.Select);
        }
    }
}