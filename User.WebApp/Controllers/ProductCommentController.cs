using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CRM.DAL.Models.DatabaseModels.Products;
using CRM.DAL.Models.DatabaseModels.ProductsComments;
using CRM.DAL.Models.DatabaseModels.ProductsUsers;
using CRM.DAL.Models.RequestModels.ProductBuy;
using CRM.User.WebApp.Models.Basic;
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
    [ODataRoutePrefix(nameof(ProductComment))]
    public class ProductCommentController : BaseController<ProductCommentController>
    {

        private readonly IMapper mapper;
        private readonly UserDbContext userDbContext;

        public ProductCommentController(ILogger<ProductCommentController> logger, UserDbContext userDbContext,
            UserManager<DAL.Models.DatabaseModels.Users.User> userManager, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(
            logger, userDbContext,
            userManager, httpContextAccessor)
        {
            this.userDbContext = userDbContext;
            this.mapper = mapper;
        }


        /// <summary>
        ///     Place product comment
        /// </summary>
        /// <response code="204">Comment placed</response>
         /// <response code="404">Product not found</response>
         /// /// <response code="400">Empty comment string</response>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] ProductComment item)
        {

            if (string.IsNullOrEmpty(item.Comment))
            {
                return BadRequest();
            }
            
            var user = await userManager.GetUserAsync(User);
            item.UserId ??= user.Id;

            var product = userDbContext.Products
                .IncludeOptimized(r=>r.ProductUsers)
                .FirstOrDefault(i => i.Id == item.ProductId);

            if (product == null)
            {
                return NotFound();
            }

            if (product.ProductUsers.All(p => p.RelationType != ProductUserRelationType.Owned))
            {
                return BadRequest();
            }

            await userDbContext.ProductComments.AddAsync(new ProductComment()
            {
                ProductId = product.Id,
                UserId = user.Id,
                Comment = item.Comment
            });
            
            await userDbContext.SaveChangesAsync();
            
            return NoContent();
        }
        
    }
}