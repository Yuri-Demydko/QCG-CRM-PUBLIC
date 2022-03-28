using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CRM.ServiceCommon.Configurations
{
    public class IdentityServerSecurityStampValidator<TUser> : SecurityStampValidator<TUser> where TUser : class
    {
        public IdentityServerSecurityStampValidator(IOptions<SecurityStampValidatorOptions> options,
            SignInManager<TUser> signInManager, ISystemClock clock, ILoggerFactory logger) : base(options,
            signInManager, clock, logger)
        {
        }

        /// <summary>
        /// Validates a security stamp of an identity as an asynchronous operation, and rebuilds the identity if the validation succeeds, otherwise rejects
        /// the identity.
        /// </summary>
        /// <param name="context">The context containing the <see cref="System.Security.Claims.ClaimsPrincipal"/>
        /// and <see cref="AuthenticationProperties"/> to validate.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous validation operation.</returns>
        public override async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            var currentUtc = DateTimeOffset.UtcNow;
            if (Clock != null)
            {
                currentUtc = Clock.UtcNow;
            }

            var issuedUtc = context.Properties.IssuedUtc;

            // Only validate if enough time has elapsed
            var validate = (issuedUtc == null);
            if (issuedUtc != null)
            {
                var timeElapsed = currentUtc.Subtract(issuedUtc.Value);
                validate = timeElapsed > Options.ValidationInterval;
            }

            if (validate)
            {
                var user = await VerifySecurityStamp(context.Principal);

                if (user == null)
                {
                    Logger.LogDebug(0, "Security stamp validation failed, rejecting cookie");
                    context.RejectPrincipal();
                    await context.HttpContext.SignOutAsync();
                }
            }
        }
    }
}