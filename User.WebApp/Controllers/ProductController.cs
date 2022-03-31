using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CRM.DAL.Models.Products;
using CRM.DAL.Models.ProductsUsers;
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
    [ODataRoutePrefix(nameof(Product))]
    public class ProductController : BaseController<ProductController>
    {

        private readonly IMapper mapper;
        private readonly UserDbContext userDbContext;
        
        public ProductController(ILogger<ProductController> logger, UserDbContext userDbContext,
            UserManager<DAL.Models.Users.User> userManager, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(
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
                .IncludeOptimized(p => p.ProductKontragents);
        }

        /// <summary>
        ///     Add product to favorites.
        /// </summary>
        /// <response code="204">The Products was successfully updated.</response>
        /// <response code="404">Product with given key not found</response>
        /// /// <response code="400">Product with given key already in favorites or not owned</response>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ODataRoute("AddToFavorites")]
        public async Task<IActionResult> PostAddToFavorites(Guid key)
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
                || item.ProductUsers.All(i => i.RelationType != ProductUserRelationType.Favorite))
            {
                return BadRequest();
            }

            
            await userDbContext.ProductUsers.AddAsync(new ProductUser()
            {
                ProductId = item.Id,
                UserId = user.Id,
                RelationType = ProductUserRelationType.Favorite
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
        [ODataRoute("RemoveFromFavorites")]
        public async Task<IActionResult> PostRemoveFromFavorites(Guid key)
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
                item.ProductUsers.Any(i=>i.RelationType==ProductUserRelationType.Favorite))
            {
                return BadRequest();
            }

            
            userDbContext.ProductUsers.Remove(item.ProductUsers.First());

            await userDbContext.SaveChangesAsync();
            
            return NoContent();
        }
        // /// <summary>
        // ///     Get current User.
        // /// </summary>
        // /// <returns>The current User.</returns>
        // /// <response code="200">The User was successfully retrieved.</response>
        // [Produces("application/json")]
        // [ProducesResponseType(typeof(DAL.Models.Users.User), StatusCodes.Status200OK)]
        // [ODataRoute("Profile")]
        // [EnableQuery]
        // public async Task<IActionResult> GetProfile()
        // {
        //     QueryIncludeOptimizedManager.AllowIncludeSubPath = true;
        //
        //     var userId = userManager.GetUserId(User);
        //     var user = await UserDbContext.Users
        //         .IncludeOptimized(i => i.UserRoles.Select(ur => ur.Role))
        //         .FirstOrDefaultAsync(i => i.Id == userId);
        //     
        //     return StatusCode(StatusCodes.Status200OK, mapper.Map<UserProfileDto>(user));
        // }

        // /// <summary>
        // ///     Get current User Policies.
        // /// </summary>
        // /// <returns>The current User Policies.</returns>
        // /// <response code="200">The Policies was successfully retrieved.</response>
        // [Produces("application/json")]
        // [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        // [ODataRoute("Policies")]
        // public IEnumerable<string> GetPolicies()
        // {
        //     return User.FindAll(ClaimTypes.UserPolicy).Select(i => i.Value);
        // }

        // /// <summary>
        // ///     Get current User Roles.
        // /// </summary>
        // /// <returns>The current User Policies.</returns>
        // /// <response code="200">The Policies was successfully retrieved.</response>
        // [Produces("application/json")]
        // [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        // [ODataRoute("Roles")]
        // public IEnumerable<string> GetRoles()
        // {
        //     return User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(i => i.Value);
        // }

        // /// <summary>
        // ///     Update User Password.
        // /// </summary>
        // /// <response code="200">The User password was successfully changed.</response>
        // [Produces("application/json")]
        // [ProducesResponseType(typeof(IdentityResult), StatusCodes.Status200OK)]
        // [ODataRoute("ChangePassword")]
        // public async Task<IdentityResult> PostChangePassword(string oldPassword, string newPassword)
        // {
        //     var user = await userManager.GetUserAsync(User);
        //
        //     return await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        // }
        
    }
}