using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.User.WebApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [HttpGet]
        [Route("[controller]/Logout")]
        public ActionResult Logout()
        {
            return SignOut(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}