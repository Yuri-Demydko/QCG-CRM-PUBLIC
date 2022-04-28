using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.User.WebApp.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		[HttpGet]
		[Route("/")]
		public IActionResult Index()
		{
			return Ok();
		}
	}
}
