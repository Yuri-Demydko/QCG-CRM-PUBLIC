using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CRM.DAL.Models.DatabaseModels.Products;
using CRM.DAL.Models.DatabaseModels.ProductsUsers;
using CRM.DAL.Models.RequestModels.ProductBuy;
using CRM.User.WebApp.Models.Basic;
using CRM.User.WebApp.Services;
using CRM.User.WebApp.Services.ProductBuy;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Z.EntityFramework.Plus;

namespace CRM.User.WebApp.Controllers
{
    [ODataRoutePrefix(nameof(ProductBuyRequest))]
    public class ProductBuyRequestController : BaseController<ProductBuyRequestController>
    {

        private readonly IMapper mapper;
        private readonly UserDbContext userDbContext;
        private readonly IProductBuyService productBuyService;
        
        public ProductBuyRequestController(ILogger<ProductBuyRequestController> logger, UserDbContext userDbContext,
            UserManager<DAL.Models.DatabaseModels.Users.User> userManager, IHttpContextAccessor httpContextAccessor, IMapper mapper, IProductBuyService productBuyService) : base(
            logger, userDbContext,
            userManager, httpContextAccessor)
        {
            this.userDbContext = userDbContext;
            this.mapper = mapper;
            this.productBuyService = productBuyService;
        }


        /// <summary>
        ///     Process purchase
        /// </summary>
        /// <response code="204">The purchase process initiated</response>
        /// <response code="404">Some of products already purchased</response>
        /// /// <response code="400">Some of products not found</response>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] ProductBuyRequest item)
        {
            var user = await userManager.GetUserAsync(User);
            item.UserId ??= user.Id;

            var alreadyOwned = await userDbContext.ProductUsers
                .AnyAsync(r => item.ProductIds.Contains(r.ProductId) && 
                               r.RelationType == ProductUserRelationType.Owned);

            if (alreadyOwned)
            {
                return BadRequest();
            }

            var notFoundProducts = (await userDbContext.Products
                .CountAsync(i => item.ProductIds.Contains(i.Id))) != item.ProductIds.Count;
            if (notFoundProducts)
            {
                return BadRequest();
            }
            
            //@TODO Переписать после уточнения работы с финансами
            var result = await productBuyService.ProcessRequestAsync(item);

            if (!result.IsSuccess) return BadRequest();
            
            var productUsersNew = item.ProductIds.Select(p =>
                new ProductUser()
                {
                    ProductId = p,
                    UserId = user.Id,
                    RelationType = ProductUserRelationType.Owned
                });
            await  userDbContext.ProductUsers.AddRangeAsync(productUsersNew);
            
            userDbContext.ProductUsers.RemoveRange(
                userDbContext.ProductUsers.Where(r=>item.ProductIds.Contains(r.ProductId)
                &&r.RelationType==ProductUserRelationType.InShoppingCart)
                );
            
            await userDbContext.SaveChangesAsync();
            return NoContent();

        }
        
    }
}