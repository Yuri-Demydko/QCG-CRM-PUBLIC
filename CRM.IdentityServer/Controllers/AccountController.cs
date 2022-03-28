using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.ReCaptcha;
using CRM.DAL.Models.Users.VerifyCodes.Enums;
using CRM.IdentityServer.Extensions.Constants;
using CRM.IdentityServer.Models;
using CRM.IdentityServer.Models.User;
using CRM.IdentityServer.Services;
using CRM.IdentityServer.ViewModels.Account;
using CRM.ServiceCommon.Helpers;
using CRM.ServiceCommon.Services;
using Hangfire;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using CRM.IdentityServer.ViewModels;
using CRM.ServiceCommon.Services.CodeService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeKit.Text;

namespace CRM.IdentityServer.Controllers
{
    [SecurityHeaders]
    [AllowAnonymous]
    [Controller]
    [DisableConcurrentExecution(10)]
    public class AccountController : Controller
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

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            // ie костыль, чтобы обрабатывать редирект
            Response.Headers.Add("Address", Request.GetDisplayUrl());

            var vm = await BuildLoginViewModelAsync(returnUrl);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            var context = await interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            // if (button != "login")
            // {
            //     if (context != null)
            //     {
            //         await interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);
            //
            //         if (context.IsNativeClient())
            //         {
            //             return this.LoadingPage("Redirect", model.ReturnUrl);
            //         }
            //
            //         return Redirect(model.ReturnUrl);
            //     }
            //
            //     return Redirect("~/");
            // }

            if (ModelState.IsValid)
            {
                var login = model.Username;
                var password = model.Password;

                User user;
                if (model.Username.Contains("@"))
                {
                    user = await userManager.FindByEmailAsync(login);
                }
                else
                {
                    //@TODO Check login
                    user = await userManager.FindByNameAsync(login);
                }

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Не удалось найти пользователя");
                    return View(await BuildLoginViewModelAsync(model));
                }

                if (!user.IsActive)
                {
                    ModelState.AddModelError(string.Empty, "Неактивный пользователь");
                    return View(await BuildLoginViewModelAsync(model));
                }


                var signIn = await signInManager.PasswordSignInAsync(user.UserName, password, false, false);
                if (signIn.Succeeded)
                {
                    await events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName,
                        clientId: context?.Client.ClientId));

                    AuthenticationProperties props = null;
                    if (AccountOptions.AllowRememberLogin && model.RememberLogin)
                    {
                        props = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                        };
                    }

                    var identityServerUser = new IdentityServerUser(user.Id)
                    {
                        DisplayName = user.UserName,
                    };

                    await HttpContext.SignInAsync(identityServerUser, props);

                    if (context != null)
                    {
                        if (context.IsNativeClient())
                        {
                            return this.LoadingPage("Redirect", model.ReturnUrl);
                        }

                        return Redirect(model.ReturnUrl);
                    }

                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        var userRole = await userManager.GetRolesAsync(user);
                        var webAppName = string.Empty;

                        if (userRole.Any(r => r == UserRoles.User))
                        {
                            webAppName = "User Web App";
                        }

                        var clientsConfig = configuration.GetSection("IdentityClients");
                        var clients = clientsConfig.GetChildren();
                        var client = clients.FirstOrDefault(c => c["ClientName"] == webAppName);
                        var url = client?.GetValue<string>("Url");
                        if (!string.IsNullOrWhiteSpace(url))
                        {
                            return Redirect(url);
                        }

                        return Redirect("~/");
                    }

                    throw new Exception("Неверный Redirect Url");
                }

                await events.RaiseAsync(new UserLoginFailureEvent(model.Username, "Ошибка входа",
                    clientId: context?.Client.ClientId));
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            }

            var vm = await BuildLoginViewModelAsync(model);
            return View(vm);
        }


        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var vm = await BuildLogoutViewModelAsync(logoutId);

            return await Logout(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

            if (User?.Identity.IsAuthenticated == true)
            {
                await signInManager.SignOutAsync();
                await events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            if (vm.TriggerExternalSignout)
            {
                string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

                return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }

            if (string.IsNullOrWhiteSpace(vm.PostLogoutRedirectUri))
            {
                return Redirect(Url.Action("Login", "Account"));
            }

            return Redirect(vm.PostLogoutRedirectUri);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
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
                    vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
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

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.NeedsContinue = true;
          
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ValidateReCaptcha]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            ViewBag.NeedsContinue = true;

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            

            var userWithName =
                await identityServerDbContext.Users.FirstOrDefaultAsync(u => u.UserName == model.Username);
            if (userWithName != null || identityServerDbContext.Users.Any(u => u.UserName == model.Username))
            {
                ViewBag.NeedsContinue = false;
                ModelState.AddModelError(nameof(model.Username), "Этот ник уже зарегистрирован!");
            }
            
            var userWithEmail =
                await identityServerDbContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (userWithEmail != null || identityServerDbContext.Users.Any(u => u.Email == model.Email))
            {
                ViewBag.NeedsContinue = false;
                ModelState.AddModelError(nameof(model.Username), "Этот адрес электронной почты уже зарегистрирован!");
            }
            
            //@TODO: EMAIL

            if (ViewBag.NeedsContinue == false)
            {
                return View(model);
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
                return View(model);
            }
            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, model.Password);

            var result = await userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                AddErrors(result);
                return View(model);
            }

            await userManager.AddToRoleAsync(user, UserRoles.DraftUser);
            await signInManager.SignInAsync(user, true);
            
            
            await identityServerDbContext.SaveChangesAsync();

            return RedirectToAction("RegisterContinue", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SendTestEmailAsync()
        {
            BackgroundJob.Enqueue<EmailSenderService>(j => j.SendTestEmail());
            return RedirectToAction("RegisterContinue", "Account");
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SendVerifyEmailForRegistration([FromBody]string email)
        {
            var codeResult = await codeService.GenerateCodeAsync(email, VerifyCodeType.Registration);
        
            if (!codeResult.IsSucceed())
            {
                return new JsonResult(new
                {
                    Code = 400,
                    Message = $"{codeResult.GetErrorsString()}"
                });
            }


            BackgroundJob.Enqueue<EmailSenderService>(j => j.SendVerifyCodeEmail(email, codeResult.Code,VerifyCodeType.Registration));
            
            return new JsonResult(new
            {
                Code = 200,
                Message = "Письмо с кодом отправлено"
            });
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SendVerifyEmailForPasswordReset([FromBody]string email)
        {
            var codeResult = await codeService.GenerateCodeAsync(email, VerifyCodeType.ForgotPassword);
        
            if (!codeResult.IsSucceed())
            {
                return new JsonResult(new
                {
                    Code = 400,
                    Message = $"{codeResult.GetErrorsString()}"
                });
            }


            BackgroundJob.Enqueue<EmailSenderService>(j => j.SendVerifyCodeEmail(email, codeResult.Code, VerifyCodeType.ForgotPassword));
            
            return new JsonResult(new
            {
                Code = 200,
                Message = "Письмо с кодом отправлено"
            });
        }

        [Authorize(Roles = UserRoles.DraftUser)]
        public async Task<ActionResult> RegisterContinue()
        {
            var user = await userManager.FindByNameAsync(User?.Identity?.Name);
            var model = new RegisterContinueViewModel()
            {
                Email = user.Email
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.DraftUser)]
        public async Task<ActionResult> RegisterContinue(RegisterContinueViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var validateCodeResult = await codeService.ValidateCodeAsync(model.Email, VerifyCodeType.Registration, model.Code);

            
            if (!validateCodeResult.IsSucceed())
            {
                if (!validateCodeResult.IsSucceed())
                {
                    ModelState.AddModelError(nameof(model.Code),validateCodeResult.GetErrorsString());
                    return View(model);
                }
            }

            var user = await userManager.FindByIdAsync(User.GetSubjectId());
            
            if (user == null)
            {
                return new JsonResult(new
                {
                    Code = 404,
                    Message = "Не удалось найти пользователя"
                });
            }
            
            user.EmailConfirmed = true;
            user.IsActive = true;
            await userManager.AddToRoleAsync(user, UserRoles.User);
            var result= await userManager.UpdateAsync(user);
            
            if (!result.Succeeded)
            {
                return new JsonResult(new
                {
                    Code = 500,
                    Message = "Не удалось сохранить в БД"
                });
            }
            

            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null||!user.EmailConfirmed)
            {
                ModelState.AddModelError(nameof(model.Email), "Не удалось найти пользователя с такой почтой!");
            }
            
            HttpContext.Items.Add("ForgotPasswordEmail",model.Email);
            
            return RedirectToAction("ForgotPasswordConfirmation", "Account", new { email = model.Email });
        }

        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation(string email)
        {
            
            if (email==null)
            {
                return BadRequest();
            }

            var model = new ForgotPasswordConfirmationViewModel()
            {
                Email = email as string
            };
            
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordConfirmation(ForgotPasswordConfirmationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var validateCodeResult = await codeService.ValidateCodeAsync(model.Email, VerifyCodeType.ForgotPassword, model.Code);

            
            if (!validateCodeResult.IsSucceed())
            {
                if (!validateCodeResult.IsSucceed())
                {
                    ModelState.AddModelError(nameof(model.Code),validateCodeResult.GetErrorsString());
                    return View(model);
                }
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            
            if (user == null)
            {
                return new JsonResult(new
                {
                    Code = 404,
                    Message = "Не удалось найти пользователя"
                });
            }
            var passwordValidator = new PasswordValidator<User>();
            var isPasswordValid = await passwordValidator.ValidateAsync(userManager, user, model.Password);
            if (!isPasswordValid.Succeeded)
            {
                ModelState.AddModelError(nameof(model.Password), "Пароль не удовлетворяет требованиям безопасности");
                return View(model);
            }
            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, model.Password);
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new JsonResult(new
                {
                    Code = 500,
                    Message = "Не удалось сохранить в БД"
                });
            }
            

            return RedirectToAction("ResetPasswordSuccess", "Account");
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordSuccess()
        {
            return View();
        }

        // [HttpPost]
        // [AllowAnonymous]
        // public async Task<ActionResult> SendVerifySmsForForgotPassword(string phone)
        // {
        //     var phoneNumberInGeneral = PhoneHelper.GetPhoneInGeneral(phone);
        //
        //     var user = identityServerDbContext.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumberInGeneral);
        //     if (user == null)
        //     {
        //         return new JsonResult(new
        //         {
        //             Code = 400,
        //             Message = $"Не удалось найти пользователя с таким телефоном!"
        //         });
        //     }
        //
        //     var smsCode = await codeService.GenerateCodeAsync(phone, VerifyCodeType.ForgotPassword);
        //     if (!smsCode.IsSucceed())
        //     {
        //         return new JsonResult(new
        //         {
        //             Code = 400,
        //             Message = $"{smsCode.GetErrorsString()}"
        //         });
        //     }
        //
        //
        //     var sendSms = await smsService.SendVerifySmsAsync(phone, smsCode.Code);
        //     if (sendSms)
        //     {
        //         return new JsonResult(new
        //         {
        //             Code = 200,
        //             Message = "Успешно отправлено смс"
        //         });
        //     }
        //
        //     return new JsonResult(new
        //     {
        //         Code = 500,
        //         Message = "Не удалось отправить смс. Обратитесь в тех. поддержку"
        //     });
        // }
        //
        // [HttpPost]
        // [AllowAnonymous]
        // public async Task<ActionResult> VerifyCodeForForgotPassword(string phone, string code)
        // {
        //     var phoneNumberInGeneral = PhoneHelper.GetPhoneInGeneral(phone);
        //
        //     var user = identityServerDbContext.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumberInGeneral);
        //     if (user == null)
        //     {
        //         return new JsonResult(new
        //         {
        //             Code = 400,
        //             Message = $"Не удалось найти пользователя с таким телефоном!"
        //         });
        //     }
        //
        //     var smsCode = await codeService.ValidateCodeAsync(phone, VerifyCodeType.ForgotPassword, code);
        //     if (!smsCode.IsSucceed())
        //     {
        //         if (!smsCode.IsSucceed())
        //         {
        //             return new JsonResult(new
        //             {
        //                 Code = 400,
        //                 Message = $"{smsCode.GetErrorsString()}"
        //             });
        //         }
        //     }
        //
        //     var res = new JsonResult(new
        //     {
        //         Code = 200,
        //         Message = "Телефон успешно подтвержден"
        //     });
        //
        //     return res;
        // }

        // [AllowAnonymous]
        // public async Task<ActionResult> ResetPassword(string phone)
        // {
        //     if (string.IsNullOrWhiteSpace(phone))
        //     {
        //         return View("Error");
        //     }
        //
        //     var phoneNumberInGeneral = PhoneHelper.GetPhoneInGeneral(phone);
        //
        //     // var isCodeWasSent = await codeService.IsCodeWasSent(phoneNumberInGeneral, VerifyCodeType.ForgotPassword);
        //     // if (!isCodeWasSent.IsSucceed())
        //     // {
        //     //     return new JsonResult(new
        //     //     {
        //     //         Code = 400,
        //     //         Message = isCodeWasSent.GetErrorsString()
        //     //     });
        //     // }
        //
        //     return View(new ResetPasswordViewModel() { Phone = phoneNumberInGeneral });
        // }
        //
        // [HttpPost]
        // [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        // public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return View(model);
        //     }
        //
        //     var phoneNumberInGeneral = PhoneHelper.GetPhoneInGeneral(model.Phone);
        //
        //     var user = await identityServerDbContext.Users.FirstOrDefaultAsync(u =>
        //         u.PhoneNumber == phoneNumberInGeneral);
        //     if (user == null)
        //     {
        //         return new JsonResult(new
        //         {
        //             Code = 500,
        //             Message = "Не удалось найти пользователя с таким телефоном!"
        //         });
        //     }
        //
        //     var passwordValidator = new PasswordValidator<User>();
        //     var isPasswordValid = await passwordValidator.ValidateAsync(userManager, user, model.Password);
        //     if (!isPasswordValid.Succeeded)
        //     {
        //         return new JsonResult(new
        //         {
        //             Code = 400,
        //             Message = string.Join(",", isPasswordValid.Errors)
        //         });
        //     }
        //
        //     var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
        //     var resetPasswordResult = await userManager.ResetPasswordAsync(user, resetToken, model.Password);
        //     if (!resetPasswordResult.Succeeded)
        //     {
        //         return new JsonResult(new
        //         {
        //             Code = 500,
        //             Message = string.Join(",", resetPasswordResult.Errors)
        //         });
        //     }
        //
        //     return RedirectToAction("ResetPasswordConfirmation", "Account");
        // }
        //
        //
        // [AllowAnonymous]
        // public ActionResult ResetPasswordConfirmation()
        // {
        //     return View();
        // }
        //
        //
        // [HttpGet]
        // [Authorize]
        // public async Task<IActionResult> ChangePassword()
        // {
        //     return View();
        // }
        //
        // [HttpPost]
        // [Authorize]
        // public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return View(model);
        //     }
        //
        //     var user = await userManager.GetUserAsync(HttpContext.User);
        //     if (user == null)
        //     {
        //         return View("Error");
        //     }
        //
        //     var result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
        //     if (!result.Succeeded)
        //     {
        //         foreach (var error in result.Errors)
        //         {
        //             ModelState.AddModelError(string.Empty, error.Description);
        //         }
        //     }
        //
        //     await signInManager.SignInAsync(user, false);
        //
        //     return View("PersonalArea");
        // }
        

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId };

            if (User?.Identity.IsAuthenticated != true)
            {
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            var logout = await interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null)
                        {
                            vm.LogoutId = await interaction.CreateLogoutContextAsync();
                        }

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return vm;
        }
    }
}