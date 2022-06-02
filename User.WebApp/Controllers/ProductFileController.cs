using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using CRM.DAL.Models.DatabaseModels.ProductFile;
using CRM.ServiceCommon.Clients;
using CRM.User.WebApp.Models.Basic;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Z.EntityFramework.Plus;

namespace CRM.User.WebApp.Controllers
{
    [ODataRoutePrefix(nameof(ProductFile))]
    public class ProductFileController : BaseController<ProductFileController>
    {

        private readonly IMapper mapper;
        private readonly SiaApiClient siaApiClient;
        public ProductFileController(ILogger<ProductFileController> logger, UserDbContext userDbContext,
            UserManager<DAL.Models.DatabaseModels.Users.User> userManager, IHttpContextAccessor httpContextAccessor, IMapper mapper, SiaApiClient siaApiClient) : base(
            logger, userDbContext,
            userManager, httpContextAccessor)
        {
            this.mapper = mapper;
            this.siaApiClient = siaApiClient;
        }

        
        
    }
}