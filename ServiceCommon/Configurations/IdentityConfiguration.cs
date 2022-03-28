using System;
using CRM.IdentityServer.Extensions.Constants;
using CRM.ServiceCommon.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CRM.ServiceCommon.Configurations
{
    public static class IdentityConfiguration
    {
        public static IdentityBuilder AddIdentityForWebApi<TUser, TRole>(
            this IServiceCollection services,
            Action<IdentityOptions> setupAction = null)
            where TUser : class
            where TRole : class
        {
            // Hosting doesn't add IHttpContextAccessor by default
            services.AddHttpContextAccessor();

            // Identity services
            services.TryAddScoped<IUserValidator<TUser>, UserValidator<TUser>>();
            services.TryAddScoped<IPasswordValidator<TUser>, PasswordValidator<TUser>>();
            services.TryAddScoped<IPasswordHasher<TUser>, PasswordHasher<TUser>>();
            services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            services.TryAddScoped<IRoleValidator<TRole>, RoleValidator<TRole>>();

            // No interface for the error describer so we can add errors without rev'ing the interface    
            services.TryAddScoped<IdentityErrorDescriber>();
            services.TryAddScoped<ISecurityStampValidator, IdentityServerSecurityStampValidator<TUser>>();
            services.TryAddScoped<IUserConfirmation<TUser>, DefaultUserConfirmation<TUser>>();
            services.TryAddScoped<UserManager<TUser>>();
            services.TryAddScoped<SignInManager<TUser>>();
            services.TryAddScoped<RoleManager<TRole>>();

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.SecurityStampClaimType = ClaimTypes.SecurityStamp;
            });
            
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromSeconds(30);
            });
            
            return new IdentityBuilder(typeof(TUser), typeof(TRole), services)
                .AddErrorDescriber<RussianLanguageIdentityErrorDescriber>();
        }
    }
}