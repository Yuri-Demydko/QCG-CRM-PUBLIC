using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.User.WebApp.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [ApiController]
    [Controller]
    [Route("api/v{apiVersion:apiVersion}/[controller]")]
    public class AccountController : Controller
    {
        [HttpGet]
        [Route("[action]")]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}