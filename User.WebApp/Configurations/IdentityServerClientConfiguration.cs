using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CRM.IdentityServer.Extensions.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using ClaimTypes = System.Security.Claims.ClaimTypes;

namespace CRM.User.WebApp.Configurations
{
    public static class IdentityServerClientConfiguration
    {
        // public static IServiceCollection ConfigureIdentityServerClient(this IServiceCollection services,
        //     IConfiguration configuration)
        // {
        //     services.AddAuthentication(options =>
        //         {
        //             options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //             options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        //             options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //             options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //         })
        //         .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        //         {
        //             options.Events = new CookieAuthenticationEvents
        //             {
        //                 OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
        //             };
        //         })
        //         .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
        //         {
        //             options.Authority = configuration.GetSection("IdentityServerClient:AuthorizationServiceUrl").Value;
        //             options.ClientId = configuration.GetSection("IdentityServerClient:ClientId").Value;
        //             options.ClientSecret = configuration.GetSection("IdentityServerClient:ClientSecret").Value;
        //             options.RequireHttpsMetadata = false;
        //
        //             options.BackchannelHttpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = delegate { return true; } };
        //
        //             options.SaveTokens = true;
        //             options.GetClaimsFromUserInfoEndpoint = true;
        //
        //             options.Scope.Add(Scopes.OpenId);
        //             options.Scope.Add(Scopes.Profile);
        //             options.Scope.Add(Scopes.Roles);
        //             options.Scope.Add(Scopes.Policies);
        //
        //             options.ClaimActions.MapJsonKey(ClaimTypes.UserRole, ClaimTypes.UserRole);
        //             options.ClaimActions.MapJsonKey(ClaimTypes.UserPolicy, ClaimTypes.UserPolicy);
        //
        //
        //             options.ResponseType = OpenIdConnectResponseType.Code;
        //             
        //
        //             options.Events.OnRedirectToIdentityProvider = ctx =>
        //             {
        //                 if (ctx.Request.Path.StartsWithSegments("/api"))
        //                 {
        //                     if (ctx.Response.StatusCode == (int) HttpStatusCode.OK)
        //                     {
        //                         ctx.Response.StatusCode = 401;
        //                     }
        //
        //                     ctx.HandleResponse();
        //                 }
        //
        //                 return Task.CompletedTask;
        //             };
        //             
        //             
        //             options.Scope.Add(Scopes.Roles);
        //             options.Scope.Add(Scopes.Policies);
        //             options.Scope.Add(Scopes.SecurityStamp);
        //         });
        //     
        //     services.AddAuthorization(options =>
        //     {
        //         options.AddPolicy(UserPolicies.RequestsAccess,
        //             policy => policy.RequireClaim(ClaimTypes.UserPolicy, UserPolicies.RequestsAccess));
        //
        //         options.AddPolicy(UserPolicies.AuctionsAccess,
        //             policy => policy.RequireClaim(ClaimTypes.UserPolicy, UserPolicies.AuctionsAccess));
        //     });
        //
        //     return services;
        // }
        
        
        public static IServiceCollection ConfigureIdentityServerClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration.GetSection("IdentityServerClient:AuthorizationServiceUrl").Value;
                    options.RequireHttpsMetadata = false;
                    options.BackchannelHttpHandler = new HttpClientHandler
                        { ServerCertificateCustomValidationCallback = delegate { return true; } };

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = false,
                        ValidateAudience = false
                    };
                    
                })
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = configuration.GetSection("IdentityServerClient:AuthorizationServiceUrl").Value;
                    options.ClientId = configuration.GetSection("IdentityServerClient:ClientId").Value;
                    options.ClientSecret = configuration.GetSection("IdentityServerClient:ClientSecret").Value;
                    options.ResponseType = OpenIdConnectResponseType.Code;
                    options.RequireHttpsMetadata = false;

                    options.BackchannelHttpHandler = new HttpClientHandler
                        { ServerCertificateCustomValidationCallback = delegate { return true; } };

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    
                    options.Scope.Add(Scopes.Roles);
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        RoleClaimType = ClaimTypes.Role,
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });

            services.AddAuthorization();

            return services;
        }
    }
}