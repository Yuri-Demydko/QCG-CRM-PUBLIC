using AutoMapper;
using CRM.DAL.Models.DatabaseModels.Products;
using CRM.ServiceCommon.Services.Files;
using CRM.User.WebApp.Models.Basic;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CRM.User.WebApp.Controllers
{
	[ODataRoutePrefix(nameof(Product))]
	public class ShopController : BaseController<ShopController>
	{
        private readonly IMapper mapper;
        private readonly UserDbContext userDbContext;

        public ShopController(ILogger<ShopController> logger, UserDbContext userDbContext,
            UserManager<DAL.Models.DatabaseModels.Users.User> userManager, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(
            logger, userDbContext,
            userManager, httpContextAccessor)
        {
            this.userDbContext = userDbContext;
            this.mapper = mapper;
        }
        /// <summary>
        ///     Get Products.
        /// </summary>
        /// <returns>The requested Products.</returns>
        /// <response code="200">The Products was successfully retrieved.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        [EnableQuery(HandleNullPropagation = HandleNullPropagationOption.False)]
        public IEnumerable<Product> Get()
        {
            QueryIncludeOptimizedManager.AllowIncludeSubPath = true;

            return userDbContext.Products
                .IncludeOptimized(p => p.Requirements)
                .IncludeOptimized(p => p.Tags)
                .IncludeOptimized(p => p.ProductKontragents)
                .IncludeOptimized(p => p.ProductComments.Select(p => p.User));
        }
    }
}
