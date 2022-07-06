using AutoMapper;
using CRM.DAL.Models.DatabaseModels.Products;
using CRM.ServiceCommon.Services.Files;
using CRM.User.WebApp.Models.Basic;
using CRM.User.WebApp.Models.Request;
using CRM.User.WebApp.Models.ViewModel;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace CRM.User.WebApp.Controllers
{
	[ODataRoutePrefix(nameof(ShopViewModel))]
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
        ///     Get Products for shop page.
        /// </summary>
        /// <returns>The requested ShopViewModel.</returns>
        /// <response code="200">The Products was successfully retrieved.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(ShopViewModel), StatusCodes.Status200OK)]
        [EnableQuery(HandleNullPropagation = HandleNullPropagationOption.False)]
        public async Task<IActionResult> ShopList([FromBody] ShopRequest request)
        {
            QueryIncludeOptimizedManager.AllowIncludeSubPath = true;
            const int objectInPage = 16;

            var product = await userDbContext.Products.Skip(objectInPage * request.CurrentPage ?? 0).Take(16).ToListAsync();
            var bestProduct = await userDbContext.Products.Skip(objectInPage * request.CurrentPage ?? 0).Take(16).ToListAsync();
            var banners = await userDbContext.Banners.ToListAsync();
            var totalPage = await userDbContext.Products.CountAsync() / objectInPage;

            return StatusCode(StatusCodes.Status200OK, new ShopViewModel(banners, bestProduct, product, totalPage, request.CurrentPage ?? 1) { });
        }
    }
}
