using CRM.IdentityServer.Extensions.Constants;
using CRM.User.WebApp.Models.Basic;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;

namespace CRM.User.WebApp.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = UserRoles.User + "," + UserRoles.Admin)]
    public abstract class BaseController<TModel> : ODataController
    {
        protected readonly ILogger<TModel> logger;
        protected readonly UserDbContext UserDbContext;
        protected readonly UserManager<DAL.Models.DatabaseModels.Users.User> userManager;

        protected BaseController(ILogger<TModel> logger,
            UserDbContext userDbContext, UserManager<DAL.Models.DatabaseModels.Users.User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.logger = logger;
            this.UserDbContext = userDbContext;
            this.userManager = userManager;

            var userId = this.userManager.GetUserId(httpContextAccessor.HttpContext.User);

            MappedDiagnosticsLogicalContext.Set("UserId", userId);

            this.UserDbContext.UserId = userId;
        }

        protected BaseController(ILogger<TModel> logger,
            UserDbContext userDbContext)
        {
            this.logger = logger;
            this.UserDbContext = userDbContext;
        }
    }
}