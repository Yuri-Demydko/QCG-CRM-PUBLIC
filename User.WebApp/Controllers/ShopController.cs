using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CRM.DAL.Models.DatabaseModels.Files;
using CRM.DAL.Models.DatabaseModels.ProductFile;
using CRM.DAL.Models.DatabaseModels.Products;
using CRM.DAL.Models.DatabaseModels.ProductsUsers;
using CRM.DAL.Models.RequestModels.ProductBuy;
using CRM.ServiceCommon.Services.Files;
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
using CRM.DAL.Models.RequestModels.Shop;
using CRM.DAL.Models.DatabaseModels.ViewModel;

namespace CRM.User.WebApp.Controllers
{
    [ODataRoutePrefix(nameof(ShopViewModel))]
    public class ShopController : BaseController<ShopController>
    {

        private readonly IMapper mapper;
        private readonly UserDbContext userDbContext;
        private readonly IFileService fileService;

        public ShopController(ILogger<ShopController> logger, UserDbContext userDbContext,
            UserManager<DAL.Models.DatabaseModels.Users.User> userManager, IHttpContextAccessor httpContextAccessor, IMapper mapper, IFileService fileService) : base(
            logger, userDbContext,
            userManager, httpContextAccessor)
        {
            this.userDbContext = userDbContext;
            this.mapper = mapper;
            this.fileService = fileService;
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
        ///     Clear shopping cart
        /// </summary>
        /// <response code="204">The Products was successfully updated.</response>
        /// <response code="404">Product with given key not found</response>
        /// /// <response code="400">Product with given key already in favorites or not owned</response>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableQuery(HandleNullPropagation = HandleNullPropagationOption.False)]
        [ODataRoute("({request})/Main")]
        public IActionResult Post([FromBody] ShopRequest request)
        {

            QueryIncludeOptimizedManager.AllowIncludeSubPath = true;
            const int objectInPage = 16;

            var product = userDbContext.Products.Skip(objectInPage * request.CurrentPage ?? 0).Take(16).ToListAsync().Result;
            var bestProduct = userDbContext.Products.Skip(objectInPage * request.CurrentPage ?? 0).Take(16).ToListAsync().Result;
            var banners = userDbContext.Banners.ToListAsync().Result;
            var totalPage = userDbContext.Products.CountAsync().Result / objectInPage;

            return StatusCode(StatusCodes.Status200OK, new ShopViewModel(banners, bestProduct, product, totalPage, request.CurrentPage ?? 1) { });
            
        }
    }
}