using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CRM.User.WebApp.Models.Basic;
using CRM.User.WebApp.Models.Basic.User.UserProfileDto;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Z.EntityFramework.Plus;
using ClaimTypes = CRM.IdentityServer.Extensions.Constants.ClaimTypes;

namespace CRM.User.WebApp.Controllers
{
    [ODataRoutePrefix(nameof(User))]
    public class UserController : BaseController<UserController>
    {

        private readonly IMapper mapper;
        public UserController(ILogger<UserController> logger, UserDbContext userDbContext,
            UserManager<DAL.Models.Users.User> userManager, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(
            logger, userDbContext,
            userManager, httpContextAccessor)
        {
            this.mapper = mapper;
        }

        /// <summary>
        ///     Get current User.
        /// </summary>
        /// <returns>The current User.</returns>
        /// <response code="200">The User was successfully retrieved.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(DAL.Models.Users.User), StatusCodes.Status200OK)]
        [ODataRoute("Profile")]
        [EnableQuery]
        public async Task<IActionResult> GetProfile()
        {
            QueryIncludeOptimizedManager.AllowIncludeSubPath = true;

            var userId = userManager.GetUserId(User);
            var user = await UserDbContext.Users
                .IncludeOptimized(i => i.UserRoles.Select(ur => ur.Role))
                .Select(i => new UserProfileDto
                {
                    Id = i.Id,
                    Email = i.Email,
                    PhoneNumber = i.PhoneNumber,
                    UserName = i.UserName,
                    IsActive = i.IsActive,
                    Roles = i.UserRoles.Select(ur => ur.Role.Name).ToList(),
                })
                .FirstOrDefaultAsync(i => i.Id == userId);
            
            return StatusCode(StatusCodes.Status200OK, user);
        }

        /// <summary>
        ///     Get current User Policies.
        /// </summary>
        /// <returns>The current User Policies.</returns>
        /// <response code="200">The Policies was successfully retrieved.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        [ODataRoute("Policies")]
        public IEnumerable<string> GetPolicies()
        {
            return User.FindAll(ClaimTypes.UserPolicy).Select(i => i.Value);
        }

        /// <summary>
        ///     Get current User Roles.
        /// </summary>
        /// <returns>The current User Policies.</returns>
        /// <response code="200">The Policies was successfully retrieved.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        [ODataRoute("Roles")]
        public IEnumerable<string> GetRoles()
        {
            return User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(i => i.Value);
        }

        /// <summary>
        ///     Update User Password.
        /// </summary>
        /// <response code="200">The User password was successfully changed.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IdentityResult), StatusCodes.Status200OK)]
        [ODataRoute("ChangePassword")]
        public async Task<IdentityResult> PostChangePassword(string oldPassword, string newPassword)
        {
            var user = await userManager.GetUserAsync(User);

            return await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }
        
    }
}