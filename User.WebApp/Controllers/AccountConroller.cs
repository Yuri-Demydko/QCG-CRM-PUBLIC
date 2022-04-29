using System.Net.Http;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using CRM.DAL.Models.RequestModels.Auth;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

        public AccountController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// User authentification
        /// </summary>
        /// <param name="request"></param>
        /// <remarks>
        ///     Token lifetime - 5 mins
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

        

            return response.IsError ? StatusCode(400, response.Error) : StatusCode(StatusCodes.Status200OK, new TokenResponse(response.AccessToken, response.RefreshToken));
        }
        
        
        /// <summary>
        /// Get new auth token by refresh token
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <remarks>
        ///     Время жизни токена авторизации - 5 минут
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
        
            if (response.IsError)
            {
                return StatusCode(400, response.Error);
            }

            return StatusCode(StatusCodes.Status200OK, new TokenResponse(response.AccessToken, response.RefreshToken));
        }
    }
}