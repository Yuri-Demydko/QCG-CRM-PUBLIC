using AutoMapper;
using Common.Enum;
using CRM.DAL.Models.DatabaseModels.Products;
using CRM.DAL.Models.DatabaseModels.ProductsUsers;
using CRM.DAL.Models.RequestModels.Shop;
using CRM.ServiceCommon.Services.Files;
using CRM.User.WebApp.Models.Basic;
using CRM.User.WebApp.Models.Request;
using CRM.User.WebApp.Models.ViewModel.Shop;
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
using Common.Attribute;

namespace CRM.User.WebApp.Controllers
{
	[ODataRoutePrefix(nameof(CRM.User.WebApp.Models.ViewModel.Shop.Shop))]
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
        [ProducesResponseType(typeof(Shop), StatusCodes.Status200OK)]
        [EnableQuery(HandleNullPropagation = HandleNullPropagationOption.False)]
        [ODataRoute()]
        public async Task<Shop> PostShopList([FromBody] ShopRequest request)
        {
            QueryIncludeOptimizedManager.AllowIncludeSubPath = true;
            const int objectInPage = 16;

            var product = new List<Product>();

           /* if(request.Genres.Any())
			{
                product = await userDbContext.Products.Where(item => request.Genres.Intersect(item.Tags).Count() != 0).ToListAsync();
            }*/
			/*else
			{
                product = await userDbContext.Products.ToListAsync();
            }*/

            if(request.FilterByPrice != null)
			{
                var optionsFilter = request.FilterByPrice.Value.GetFilterByPriceInfo();

                if(optionsFilter.HasDiscount == true)
				{
                    product = product.Where(item => item.DiscountPrice != null).ToList();
                }
                if (optionsFilter.IsFrom == true)
                {
                    product = product.Where(item => item.Price >= optionsFilter.Price).ToList();
                }
                else
                {
                    product = product.Where(item => item.Price <= optionsFilter.Price).ToList();
                }
            }

            if (request.SortType == SortType.PopularFirst)
			{
                product = product.OrderBy(item => item.ProductUsers.Count(i => i.RelationType == ProductUserRelationType.Like)).ToList();
			}
            if (request.SortType == SortType.PriceDescending)
            {
                product = product.OrderByDescending(item => item.Price).ToList();
            }
            if (request.SortType == SortType.PopularFirst)
            {
                product = product.OrderBy(item => item.Price).ToList();
            }
           
            product = product.Skip(objectInPage * request.CurrentPage ?? 0).Take(16).ToList();
            var bestProduct = await userDbContext.Products.Skip(objectInPage * request.CurrentPage ?? 0).Take(16).ToListAsync();
            var banners = await userDbContext.Banners.ToListAsync();
            var totalPage = await userDbContext.Products.CountAsync() / objectInPage;

            return new Shop(banners, bestProduct, product, totalPage, request.CurrentPage ?? 1);
        }
    }
}
