using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CRM.DAL.Models.DatabaseModels.Products;
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
    [ODataRoutePrefix(nameof(ProductUser))]
    public class ProductUserController : BaseController<ProductUserController>
    {

        private readonly IMapper mapper;
        private readonly UserDbContext userDbContext;

        public ProductUserController(ILogger<ProductUserController> logger, UserDbContext userDbContext,
            UserManager<DAL.Models.DatabaseModels.Users.User> userManager, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(
            logger, userDbContext,
            userManager, httpContextAccessor)
        {
            this.userDbContext = userDbContext;
            this.mapper = mapper;
        }


        /// <summary>
        ///     Get ProductUsers.
        /// </summary>
        /// <returns>The requested ProductUsers.</returns>
        /// <response code="200">The ProductUsers was successfully retrieved.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ProductUser>), StatusCodes.Status200OK)]
        [EnableQuery(HandleNullPropagation = HandleNullPropagationOption.False)]
        public async Task<IEnumerable<ProductUser>> Get()
        {
            QueryIncludeOptimizedManager.AllowIncludeSubPath = true;

            var user = await userManager.GetUserAsync(User);
            
            return userDbContext.ProductUsers
                .IncludeOptimized(r => r.Product.Requirements)
                .IncludeOptimized(r => r.Product.Tags)
                .IncludeOptimized(r => r.Product.ProductComments.Select(r => r.User))
                .IncludeOptimized(r => r.Product.ProductFiles)
                .IncludeOptimized(r => r.Product.ProductKontragents.Select(r => r.Kontragent))
                .Where(r=>r.UserId==user.Id);
        }
        
    }
}