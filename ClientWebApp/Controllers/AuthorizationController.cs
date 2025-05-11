using Microsoft.AspNetCore.Mvc;

namespace ClientWebApp.Controllers
{
	public class AuthorizationController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Registration()
		{
			return View();
		}
    }
}
