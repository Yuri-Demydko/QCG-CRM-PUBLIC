using System;
using System.Linq;
using System.Threading.Tasks;
using CRM.DAL.Models.DatabaseModels.Users;
using CRM.DAL.Models.DatabaseModels.Users.VerifyCodes.Enums;
using CRM.IdentityServer.Extensions.Constants;
using CRM.IdentityServer.Models;
using CRM.IdentityServer.Services;
using CRM.IdentityServer.ViewModels;
using CRM.IdentityServer.ViewModels.Account;
using CRM.ServiceCommon.Services;
using CRM.ServiceCommon.Services.CodeService;
using Hangfire;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CRM.IdentityServer.Controllers.Api
{
    [SecurityHeaders]
    [AllowAnonymous]
    [Controller]
    [DisableConcurrentExecution(10)]
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{apiVersion:apiVersion}/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IIdentityServerInteractionService interaction;
        private readonly IClientStore clientStore;
        private readonly IAuthenticationSchemeProvider schemeProvider;
        private readonly IEventService events;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IdentityServerDbContext identityServerDbContext;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;
        private readonly ICodeService codeService;


        public AccountController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IdentityServerDbContext identityServerDbContext,
            IEmailService emailService,
            IConfiguration configuration, ICodeService codeService)
        {
            this.interaction = interaction;
            this.clientStore = clientStore;
            this.schemeProvider = schemeProvider;
            this.events = events;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.identityServerDbContext = identityServerDbContext;
            this.emailService = emailService;
            this.configuration = configuration;
            this.codeService = codeService;
        }

        [HttpPost]
        [Route("[action]")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            var needsContinue = true;

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                    .Where(y => y.Count > 0)
                    .ToList();

                return BadRequest(errors);
            }


            var userWithName =
                await identityServerDbContext.Users.FirstOrDefaultAsync(u => u.UserName == model.Username);
            if (userWithName != null || identityServerDbContext.Users.Any(u => u.UserName == model.Username))
            {
                needsContinue = false;
                ModelState.AddModelError(nameof(model.Username), "Этот ник уже зарегистрирован!");
            }

            var userWithEmail =
                await identityServerDbContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (userWithEmail != null || identityServerDbContext.Users.Any(u => u.Email == model.Email))
            {
                needsContinue = false;
                ModelState.AddModelError(nameof(model.Username), "Этот адрес электронной почты уже зарегистрирован!");
            }

            //@TODO: EMAIL

            if (needsContinue == false)
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                    .Where(y => y.Count > 0)
                    .ToList();

                return BadRequest(errors);
            }

            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                EmailConfirmed = false,
                IsActive = false,
            };
            var passwordValidator = new PasswordValidator<User>();
            var isPasswordValid = await passwordValidator.ValidateAsync(userManager, user, model.Password);
            if (!isPasswordValid.Succeeded)
            {
                ModelState.AddModelError(nameof(model.Password), "Пароль не удовлетворяет требованиям безопасности");
                var errors = ModelState.Select(x => x.Value.Errors)
                    .Where(y => y.Count > 0)
                    .ToList();

                return BadRequest(errors);
            }

            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, model.Password);

            var result = await userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                    .Where(y => y.Count > 0)
                    .ToList();
                return BadRequest(errors);
            }

            await userManager.AddToRoleAsync(user, UserRoles.DraftUser);
            await signInManager.SignInAsync(user, true);


            await identityServerDbContext.SaveChangesAsync();

            return Ok("You can continue registration"); //RedirectToAction("RegisterContinue", "Account");
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.DraftUser)]
        [Route("[action]")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> RegisterContinue(RegisterContinueViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                    .Where(y => y.Count > 0)
                    .ToList();

                return BadRequest(errors);
            }

            var validateCodeResult =
                await codeService.ValidateCodeAsync(model.Email, VerifyCodeType.Registration, model.Code);


            if (!validateCodeResult.IsSucceed())
            {
                if (!validateCodeResult.IsSucceed())
                {
                    ModelState.AddModelError(nameof(model.Code), validateCodeResult.GetErrorsString());
                    var errors = ModelState.Select(x => x.Value.Errors)
                        .Where(y => y.Count > 0)
                        .ToList();

                    return BadRequest(errors);
                }
            }

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return NotFound("Can't find user");
            }

            user.EmailConfirmed = true;
            user.IsActive = true;
            await userManager.AddToRoleAsync(user, UserRoles.User);
            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return new StatusCodeResult(500);
            }


            return Ok("Registration complete");
            //return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("[action]")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> SendVerifyEmailForRegistration([FromQuery] string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null||user.EmailConfirmed)
            {
                return NotFound("Не удалось найти данного пользователя с неподтвержденной почтой!");
            }
            
            var codeResult = await codeService.GenerateCodeAsync(email, VerifyCodeType.Registration);

            if (!codeResult.IsSucceed())
            {
                return BadRequest($"{codeResult.GetErrorsString()}");
            }


            BackgroundJob.Enqueue<EmailSenderService>(j =>
                j.SendVerifyCodeEmail(email, codeResult.Code, VerifyCodeType.Registration));

            return Ok("Письмо с кодом отправлено");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("[action]")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> SendVerifyEmailForPasswordReset([FromQuery] string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null||!user.EmailConfirmed)
            {
                return NotFound("Не удалось найти пользователя с такой почтой!");
            }
            
            var codeResult = await codeService.GenerateCodeAsync(email, VerifyCodeType.ForgotPassword);

            if (!codeResult.IsSucceed())
            {
                return BadRequest($"{codeResult.GetErrorsString()}");
            }


            BackgroundJob.Enqueue<EmailSenderService>(j =>
                j.SendVerifyCodeEmail(email, codeResult.Code, VerifyCodeType.ForgotPassword));

            return Ok("Письмо с кодом отправлено");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("[action]")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ForgotPasswordConfirmation(ForgotPasswordConfirmationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var validateCodeResult =
                await codeService.ValidateCodeAsync(model.Email, VerifyCodeType.ForgotPassword, model.Code);


            if (!validateCodeResult.IsSucceed())
            {
                ModelState.AddModelError(nameof(model.Code), validateCodeResult.GetErrorsString());
                return BadRequest(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null||!user.EmailConfirmed)
            {
                return NotFound("Не удалось найти пользователя с такой почтой!");
            }

            var passwordValidator = new PasswordValidator<User>();
            var isPasswordValid = await passwordValidator.ValidateAsync(userManager, user, model.Password);
            if (!isPasswordValid.Succeeded)
            {
                ModelState.AddModelError(nameof(model.Password), "Пароль не удовлетворяет требованиям безопасности");
                return BadRequest(model);
            }

            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, model.Password);
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Не удалось сохранить в БД");
            }


            return Ok("Password reset complete");
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null && await schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == IdentityServer4.IdentityServerConstants.LocalIdentityProvider;

                var vm = new LoginViewModel
                {
                    EnableLocalLogin = local,
                    ReturnUrl = returnUrl,
                    Username = context?.LoginHint,
                };

                if (!local)
                {
                    vm.ExternalProviders = new[] {new ExternalProvider {AuthenticationScheme = context.IdP}};
                }

                return vm;
            }

            var schemes = await schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null)
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName ?? x.Name,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;
            if (context?.Client.ClientId != null)
            {
                var client = await clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider =>
                            client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }
    }
}