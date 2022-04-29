using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CRM.DAL.Models.DatabaseModels.EmailChanges;
using CRM.DAL.Models.DatabaseModels.Users.VerifyCodes.Enums;
using CRM.DAL.Models.RequestModels.ChangeEmail;
using CRM.DAL.Models.RequestModels.ChangePassword;
using CRM.IdentityServer.Extensions.Constants;
using CRM.ServiceCommon.Services.CodeService;
using CRM.User.WebApp.Models.Basic;
using CRM.User.WebApp.Models.Basic.User.UserProfileDto;
using CRM.User.WebApp.Models.Request;
using CRM.User.WebApp.Services;
using Hangfire;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ICodeService codeService;
       
        public UserController(ILogger<UserController> logger, UserDbContext userDbContext,
            UserManager<DAL.Models.DatabaseModels.Users.User> userManager, IHttpContextAccessor httpContextAccessor, IMapper mapper, ICodeService codeService) : base(
            logger, userDbContext,
            userManager, httpContextAccessor)
        {
            this.mapper = mapper;
            this.codeService = codeService;
        }

        /// <summary>
        ///     Get current User.
        /// </summary>
        /// <returns>The current User.</returns>
        /// <response code="200">The User was successfully retrieved.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(DAL.Models.DatabaseModels.Users.User), StatusCodes.Status200OK)]
        [ODataRoute("Profile")]
        [EnableQuery]
        public async Task<IActionResult> GetProfile()
        {
            QueryIncludeOptimizedManager.AllowIncludeSubPath = true;

            var userId = userManager.GetUserId(User);
            var user = await UserDbContext.Users
                .IncludeOptimized(i => i.UserRoles.Select(ur => ur.Role))
                .IncludeOptimized(i=>i.KontragentUsers.Select(r=>r.Kontragent))
                .IncludeOptimized(i=>i.ProductUsers.Select(r=>r.Product))
                .IncludeOptimized(r=>r.ProductComments)
                .IncludeOptimized(r=>r.UserClaims)
                .FirstOrDefaultAsync(i => i.Id == userId);
            
            return StatusCode(StatusCodes.Status200OK, mapper.Map<UserProfileDto>(user));
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
        public async Task<IdentityResult> PostChangePassword([FromBody] ChangePasswordRequest request)
        {
            var user = await userManager.GetUserAsync(User);

            var result= await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            await UserDbContext.SaveChangesAsync();
            return result;
        }

        /// <summary>
        ///     Update User Email.
        /// </summary>
        /// <response code="200">The User password was successfully changed.</response>
         /// <response code="400">The User email same with previous.</response>
         /// <response code="400">Code generating error.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(JsonResult), StatusCodes.Status200OK)]
        [ODataRoute("RequestEmailChange")]
        public async Task<ActionResult> PostRequestEmailChange([FromBody] ChangeEmailRequest request)
        {

            var user = await userManager.GetUserAsync(User);

            if (request.NewEmail == user.Email)
            {
                return new JsonResult(new
                {
                    Code = 400,
                    Message = $"New email same with previous"
                });
            }
            
            if (await userManager.FindByEmailAsync(request.NewEmail)!=null)
            {
                return new JsonResult(new
                {
                    Code = 400,
                    Message = $"User with such email already exists"
                });
            }

            var codeResult = await codeService.GenerateCodeAsync(user.Email, VerifyCodeType.ChangeEmail);

            if (!codeResult.IsSucceed())
            {
                return new JsonResult(new
                {
                    Code = 400,
                    Message = $"{codeResult.GetErrorsString()}"
                });
            }


            BackgroundJob.Enqueue<EmailSenderService>(j =>
                j.SendVerifyCodeEmail(request.NewEmail, codeResult.Code, VerifyCodeType.ChangeEmail));

            await UserDbContext.EmailChanges.AddAsync(new EmailChange
            {
                UserId = user.Id,
                NewEmail = request.NewEmail,
                Confirmed = false,
                CreatedAt = DateTime.Now,
                UserToken = await userManager.GenerateChangeEmailTokenAsync(user,request.NewEmail)
            });
            await UserDbContext.SaveChangesAsync();
            
            return new JsonResult(new
            {
                Code = 200,
                Message = "Письмо с кодом подтверждения отправлено"
            });
        }

        /// <summary>
        ///     Update User Email.
        /// </summary>
        /// <response code="200">The User password was successfully changed.</response>
        /// <response code="400">The User email same with previous.</response>
        /// <response code="400">Code generating error.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(JsonResult), StatusCodes.Status204NoContent)]
        [ODataRoute("ConfirmEmailChange")]
        public async Task<ActionResult> PostConfirmEmailChange([FromBody] ChangeEmailConfirmationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await userManager.GetUserAsync(User);

            var validateCodeResult =
                await codeService.ValidateCodeAsync(user.Email, VerifyCodeType.ChangeEmail, request.Code);


            if (!validateCodeResult.IsSucceed())
            {
                return BadRequest(validateCodeResult.GetErrorsString());
            }

            var emailChange = await UserDbContext
                .EmailChanges
                .OrderByDescending(i => i.CreatedAt)
                .FirstOrDefaultAsync(i => i.UserId == user.Id
                                          && !i.Confirmed);

            var result = await userManager.ChangeEmailAsync(user, emailChange.NewEmail, emailChange.UserToken);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            emailChange.Confirmed = true;

            await UserDbContext.SaveChangesAsync();

            return NoContent();
        }
        
    }
}