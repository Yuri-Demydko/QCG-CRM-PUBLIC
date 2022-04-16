using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CRM.DAL.Models.DatabaseModels.Products;
using CRM.DAL.Models.DatabaseModels.ProductsUsers;
using CRM.DAL.Models.RequestModels.ProductBuy;
using CRM.User.WebApp.Models.Basic;
using CRM.User.WebApp.Models.UnnecessaryModels;
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
    [ODataRoutePrefix(nameof(Product))]
    public class ProductController : BaseController<ProductController>
    {

        private readonly IMapper mapper;
        private readonly UserDbContext userDbContext;
        
        public ProductController(ILogger<ProductController> logger, UserDbContext userDbContext,
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
                .IncludeOptimized(p => p.ProductComments.Select(p=>p.User));
        }
        
        /// <summary>
        ///     Get Product.
        /// </summary>
        /// <returns>The requested Product.</returns>
        /// <response code="200">The Product was successfully retrieved.</response>
        /// /// <response code="404">The Product was not found</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EnableQuery(HandleNullPropagation = HandleNullPropagationOption.False)]
        public async Task<IActionResult> Get(Guid key)
        {
            QueryIncludeOptimizedManager.AllowIncludeSubPath = true;

            var item= await userDbContext.Products
                .IncludeOptimized(p => p.Requirements)
                .IncludeOptimized(p => p.Tags)
                .IncludeOptimized(p => p.ProductKontragents)
                .IncludeOptimized(p=>p.ProductFiles)
                .IncludeOptimized(p=>p.ProductUsers)
                .IncludeOptimized(p => p.ProductComments.Select(p=>p.User))
                .FirstOrDefaultAsync(i=>i.Id==key);
            if (item == null)
            {
                return NotFound();
            }

            return StatusCode(StatusCodes.Status200OK, item);
        }
        
        /// <summary>
        ///     Reserve api to calculate shopping cart total price.
        /// </summary>
        /// <returns>Total products price.</returns>
        /// <response code="200">Price was retrieved.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ODataRoute("CartTotalPrices")]
        public ShoppingCartPriceAggregationResult GetCartTotalPrices()
        {
            var userId = userManager.GetUserId(User);
            var items = userDbContext.Products
                .IncludeOptimized(p => p.ProductUsers)
                .Where(r => r.ProductUsers.Any(pu =>
                    pu.UserId == userId && pu.RelationType == ProductUserRelationType.InShoppingCart));

            return new ShoppingCartPriceAggregationResult()
            {
                TotalPrice = items.Sum(i => i.DiscountPrice ?? i.Price),
                TotalDiscount = items.Sum(i => i.Price-i.DiscountPrice.GetValueOrDefault()),
                TotalRawPrice = items.Sum(i => i.Price)
            };
        }
        
        /// <summary>
        ///     Clear shopping cart
        /// </summary>
        /// <response code="204">The Products was successfully updated.</response>
        /// <response code="404">Product with given key not found</response>
        /// /// <response code="400">Product with given key already in favorites or not owned</response>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ODataRoute("ClearCart")]
        public async Task<IActionResult> PostClearCart()
        {
            var user = await userManager.GetUserAsync(User);

            var items = userDbContext.ProductUsers
                .Where(r => r.UserId == user.Id && r.RelationType == ProductUserRelationType.InShoppingCart)
                .ToList();

            userDbContext.ProductUsers.RemoveRange(items);

            await userDbContext.SaveChangesAsync();
            
            return NoContent();
        }
        
        
        /// <summary>
        ///     Add product to shopping cart.
        /// </summary>
        /// <response code="204">The Products was successfully updated.</response>
        /// <response code="404">Product with given key not found</response>
        /// /// <response code="400">Product with given key already in favorites or not owned</response>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ODataRoute("({key})/AddToCart")]
        public async Task<IActionResult> PostAddToCart(Guid key)
        {
            var user = await userManager.GetUserAsync(User);
            
            var item = await userDbContext.Products
                .IncludeOptimized(p => p.ProductUsers
                    .Where(pu=>pu.UserId==user.Id))
                .FirstOrDefaultAsync(i => i.Id == key);
        
            if (item == null)
            {
                return NotFound();
            }
        
            if (item.ProductUsers.Any(i => i.RelationType == ProductUserRelationType.Owned) 
                || item.ProductUsers.Any(i => i.RelationType == ProductUserRelationType.InShoppingCart))
            {
                return BadRequest();
            }
        
            
            await userDbContext.ProductUsers.AddAsync(new ProductUser()
            {
                ProductId = item.Id,
                UserId = user.Id,
                RelationType = ProductUserRelationType.InShoppingCart
            });
        
            await userDbContext.SaveChangesAsync();
            
            return NoContent();
        }
        
        /// <summary>
        ///     Add product to favorites.
        /// </summary>
        /// <response code="204">The Products was successfully updated.</response>
        /// <response code="404">Product with given key not found</response>
        /// /// <response code="400">Product with given key already in favorites or not owned</response>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ODataRoute("({key})/RemoveFromCart")]
        public async Task<IActionResult> PostRemoveFromCart(Guid key)
        {
            var user = await userManager.GetUserAsync(User);
            
            var item = await userDbContext.Products
                .IncludeOptimized(p => p.ProductUsers
                    .Where(pu=>pu.UserId==user.Id))
                .FirstOrDefaultAsync(i => i.Id == key);
        
            if (item == null)
            {
                return NotFound();
            }
        
            if (item.ProductUsers.Any(i => i.RelationType == ProductUserRelationType.Owned) 
                || item.ProductUsers.All(i => i.RelationType != ProductUserRelationType.InShoppingCart))
            {
                return BadRequest();
            }
        
            userDbContext.ProductUsers.Remove(item.ProductUsers.First(i=>i.RelationType==ProductUserRelationType.InShoppingCart));
        
            await userDbContext.SaveChangesAsync();
            
            return NoContent();
        }
        
        /// <summary>
        ///     Add product to liked
        /// </summary>
        /// <response code="204">The Products was successfully updated.</response>
        /// <response code="404">Product with given key not found</response>
        /// /// <response code="400">Product with given key already in liked or not owned</response>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ODataRoute("({key})/Like")]
        public async Task<ActionResult> PostLike(Guid key)
        {
            var user = await userManager.GetUserAsync(User);
            
            var item = await userDbContext.Products
                .IncludeOptimized(p => p.ProductUsers
                    .Where(pu=>pu.UserId==user.Id))
                .FirstOrDefaultAsync(i => i.Id == key);
        
            if (item == null)
            {
                return NotFound();
            }
        
            if (item.ProductUsers.All(i => i.RelationType != ProductUserRelationType.Owned) 
                || item.ProductUsers.Any(i => i.RelationType == ProductUserRelationType.Like))
            {
                return BadRequest();
            }


            var dislike = item.ProductUsers.FirstOrDefault(i => i.RelationType == ProductUserRelationType.Dislike);
            if (dislike != null)
            {
                userDbContext.ProductUsers.Remove(dislike);
            }
            
            
            await userDbContext.ProductUsers.AddAsync(new ProductUser()
            {
                ProductId = item.Id,
                UserId = user.Id,
                RelationType = ProductUserRelationType.Like
            });
        
            await userDbContext.SaveChangesAsync();
            
            return NoContent();
        }
        
        /// <summary>
        ///     Add product to disliked.
        /// </summary>
        /// <response code="204">The Products was successfully updated.</response>
        /// <response code="404">Product with given key not found</response>
        /// /// <response code="400">Product with given key already in disliked or not owned</response>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ODataRoute("({key})/Dislike")]
        public async Task<ActionResult> PostDislike(Guid key)
        {
            var user = await userManager.GetUserAsync(User);
            
            var item = await userDbContext.Products
                .IncludeOptimized(p => p.ProductUsers
                    .Where(pu=>pu.UserId==user.Id))
                .FirstOrDefaultAsync(i => i.Id == key);
        
            if (item == null)
            {
                return NotFound();
            }
        
            if (item.ProductUsers.All(i => i.RelationType != ProductUserRelationType.Owned) 
                || item.ProductUsers.Any(i => i.RelationType == ProductUserRelationType.Dislike))
            {
                return BadRequest();
            }

            var like = item.ProductUsers.FirstOrDefault(i => i.RelationType == ProductUserRelationType.Like);
            if (like != null)
            {
                userDbContext.ProductUsers.Remove(like);
            }
            
            await userDbContext.ProductUsers.AddAsync(new ProductUser()
            {
                ProductId = item.Id,
                UserId = user.Id,
                RelationType = ProductUserRelationType.Dislike
            });
        
            await userDbContext.SaveChangesAsync();
            
            return NoContent();
        }
        
        /// <summary>
        ///     Remove like or dislike
        /// </summary>
        /// <response code="204">The Products was successfully updated.</response>
        /// <response code="404">Product with given key not found</response>
        /// /// <response code="400">Product with given key already hasn't like or dislike or not owned</response>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ODataRoute("({key})/RemoveReaction")]
        public async Task<IActionResult> PostRemoveReaction(Guid key)
        {
            var user = await userManager.GetUserAsync(User);
            
            var item = await userDbContext.Products
                .IncludeOptimized(p => p.ProductUsers
                    .Where(pu=>pu.UserId==user.Id))
                .FirstOrDefaultAsync(i => i.Id == key);
        
            if (item == null)
            {
                return NotFound();
            }
        
            if (item.ProductUsers.All(i => i.RelationType != ProductUserRelationType.Owned) ||
                !item.ProductUsers.Any(i=>i.RelationType==ProductUserRelationType.Like||i.RelationType!=ProductUserRelationType.Dislike))
            {
                return BadRequest();
            }
        
            
            userDbContext.ProductUsers.Remove(item.ProductUsers.First(i=>
                i.RelationType==ProductUserRelationType.Like
                ||i.RelationType==ProductUserRelationType.Dislike
                ));
        
            await userDbContext.SaveChangesAsync();
            
            return NoContent();
        }
        
        
    }
}