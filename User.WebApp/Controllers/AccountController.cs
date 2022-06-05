using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using AutoMapper;
using CRM.DAL.Models.RequestModels.Auth;
using CRM.DAL.Models.ResponseModels.Auth;
using CRM.User.WebApp.Models.Basic;
using DelegateDecompiler.EntityFrameworkCore;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Z.EntityFramework.Plus;
using TokenResponse = CRM.DAL.Models.ResponseModels.Auth.TokenResponse;

namespace CRM.User.WebApp.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [ApiController]
    [Controller]
    [Route("api/v{apiVersion:apiVersion}/[controller]")]
    public class AccountController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly UserDbContext userDbContext;

        public AccountController(IConfiguration configuration, UserManager<DAL.Models.DatabaseModels.Users.User> userManager, IMapper mapper, UserDbContext userDbContext)
        {
            this.configuration = configuration;
            this.userDbContext = userDbContext;
        }

        /// <summary>
        /// User authentification
        /// </summary>
        /// <param name="request"></param>
        /// <remarks>
        ///     Token lifetime - 1 day
        /// </remarks>
        /// <response code="200">Auth and Refresh tokens</response>
        /// <response code="400">Validation error</response>
        /// <response code="401">Unable to authentificate user</response>
        /// <response code="404">User not found</response>
        /// <response code="404">User roles not found</response>
        /// <response code="500">User token refresh error</response>
        [ProducesResponseType(typeof(TokenResponse), 200)]
        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        [Route("[action]")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            var (isValid, errorResponse) = request.Validate();
        
            if (!isValid)
            {
                return StatusCode(400, errorResponse);
            }
        
            var clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            var client = new HttpClient(clientHandler);
        
            var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = configuration.GetSection("IdentityServerClient:AuthorizationServiceUrl").Value +
                          "/connect/token",
                ClientId = configuration.GetSection("IdentityServerClient:ClientId").Value,
                ClientSecret = configuration.GetSection("IdentityServerClient:ClientSecret").Value,
                UserName = request.Login,
                Password = request.Password
            });

            if (!response.IsError)
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(response.AccessToken);
                var tokenS = jsonToken as JwtSecurityToken;
                var userId = tokenS!.Claims.First(r => r.Type == "userId").Value;

                var user = await userDbContext.Users
                    .IncludeOptimized(r=>r.UserRoles)
                    .IncludeOptimized(i => i.UserRoles.Select(ur => ur.Role))
                    .IncludeOptimized(r => r.UserClaims)
                    .IncludeOptimized(r => r.SiaAddresses)
                    .DecompileAsync()
                    .FirstOrDefaultAsync(r => r.Id == userId);
                
                

                return StatusCode(StatusCodes.Status200OK, new AuthSuccessResponse()
                {
                    Tokens = new TokenResponse(response.AccessToken, response.RefreshToken, DateTime.Now.AddDays(1)),
                    User = new UserAuthResponseModel()
                    {
                        IsActive = user.IsActive,
                        RegistrationDate = user.RegistrationDate,
                         Roles = user.UserRoles.Select(r=>r.Role.Name),
                         LastSiaAddress = user.LastSiaAddress,
                        SiaCoinBalance = user.SiaCoinBalance,
                        Id = user.Id,
                        Email = user.Email,
                        EmailConfirmed = user.EmailConfirmed,
                        UserName = user.UserName  
                    }
                });

            }
            
            return StatusCode(400, response.Error);
        }
        
        
        /// <summary>
        /// Get new auth token by refresh token
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <remarks>
        ///     Время жизни токена авторизации - 1 день
        /// </remarks>
        /// <response code="200">Auth and Refresh tokens</response>
        /// <response code="404">User not found</response>
        /// <response code="404">User's roles not found</response>
        /// <response code="500">Token refresh error</response>
        [ProducesResponseType(typeof(TokenResponse), 200)]
        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        [Route("[action]")]
        public async Task<ActionResult> Refresh([FromBody] string refreshToken)
        {
            var clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            var client = new HttpClient(clientHandler);
        
            var response = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = configuration.GetSection("IdentityServerClient:AuthorizationServiceUrl").Value +
                          "/connect/token",
                ClientId = configuration.GetSection("IdentityServerClient:ClientId").Value,
                ClientSecret = configuration.GetSection("IdentityServerClient:ClientSecret").Value,
        
                RefreshToken = refreshToken
            });

            if (!response.IsError)
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(response.AccessToken);
                var tokenS = jsonToken as JwtSecurityToken;
                var userId = tokenS!.Claims.First(r => r.Type == "userId").Value;

                var user = await userDbContext.Users
                    .IncludeOptimized(r => r.UserRoles)
                    .IncludeOptimized(i => i.UserRoles.Select(ur => ur.Role))
                    .IncludeOptimized(r => r.UserClaims)
                    .IncludeOptimized(r => r.SiaAddresses)
                    .DecompileAsync()
                    .FirstOrDefaultAsync(r => r.Id == userId);


                return StatusCode(StatusCodes.Status200OK, new AuthSuccessResponse()
                {
                    Tokens = new TokenResponse(response.AccessToken, response.RefreshToken, DateTime.Now.AddDays(1)),
                    User = new UserAuthResponseModel()
                    {
                        IsActive = user.IsActive,
                        RegistrationDate = user.RegistrationDate,
                        Roles = user.UserRoles.Select(r => r.Role.Name),
                        LastSiaAddress = user.LastSiaAddress,
                        SiaCoinBalance = user.SiaCoinBalance,
                        Id = user.Id,
                        Email = user.Email,
                        EmailConfirmed = user.EmailConfirmed,
                        UserName = user.UserName
                    }
                });
            }

            return StatusCode(400, response.Error);
        }
    }
}