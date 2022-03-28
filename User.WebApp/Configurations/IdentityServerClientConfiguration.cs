using System.Net.Http;
using System.Threading.Tasks;
using CRM.IdentityServer.Extensions.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using ClaimTypes = CRM.IdentityServer.Extensions.Constants.ClaimTypes;

namespace CRM.User.WebApp.Configurations
{
    public static class IdentityServerClientConfiguration
    {
        public static IServiceCollection ConfigureIdentityServerClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
                    };
                })
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = configuration.GetSection("IdentityServerClient:AuthorizationServiceUrl").Value;
                    options.ClientId = configuration.GetSection("IdentityServerClient:ClientId").Value;
                    options.ClientSecret = configuration.GetSection("IdentityServerClient:ClientSecret").Value;
                    options.RequireHttpsMetadata = false;

                    options.BackchannelHttpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = delegate { return true; } };

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.Scope.Add(Scopes.OpenId);
                    options.Scope.Add(Scopes.Profile);
                    options.Scope.Add(Scopes.Roles);
                    options.Scope.Add(Scopes.Policies);

                    options.ClaimActions.MapJsonKey(ClaimTypes.UserRole, ClaimTypes.UserRole);
                    options.ClaimActions.MapJsonKey(ClaimTypes.UserPolicy, ClaimTypes.UserPolicy);


                    options.ResponseType = OpenIdConnectResponseType.Code;
                   
                    options.Events.OnRedirectToIdentityProvider = context =>
                    {
                        context.Properties.RedirectUri = "/";
                        return Task.CompletedTask;
                    };
                    
                    options.Scope.Add(Scopes.Roles);
                    options.Scope.Add(Scopes.Policies);
                    options.Scope.Add(Scopes.SecurityStamp);
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(UserPolicies.RequestsAccess,
                    policy => policy.RequireClaim(ClaimTypes.UserPolicy, UserPolicies.RequestsAccess));

                options.AddPolicy(UserPolicies.AuctionsAccess,
                    policy => policy.RequireClaim(ClaimTypes.UserPolicy, UserPolicies.AuctionsAccess));
            });

            return services;
        }
    }
}