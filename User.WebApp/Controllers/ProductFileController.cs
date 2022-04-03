using AutoMapper;
using CRM.DAL.Models.DatabaseModels.ProductFile;
using CRM.User.WebApp.Models.Basic;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CRM.User.WebApp.Controllers
{
    [ODataRoutePrefix(nameof(ProductFile))]
    public class ProductFileController : BaseController<ProductFileController>
    {

        private readonly IMapper mapper;
        public ProductFileController(ILogger<ProductFileController> logger, UserDbContext userDbContext,
            UserManager<DAL.Models.DatabaseModels.Users.User> userManager, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(
            logger, userDbContext,
            userManager, httpContextAccessor)
        {
            this.mapper = mapper;
        }

        
        
    }
}