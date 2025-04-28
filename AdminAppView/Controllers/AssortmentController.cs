using AdminApp.DatabaseControllers;
using AdminAppView.Models;
using DatabaseAccess;
using Microsoft.AspNetCore.Mvc;

namespace AdminAppView.Controllers
{
	public class AssortmentController(AssortementSetupApplicationContext dbContext) : Controller
	{
		private readonly AssortementSetupApplicationContext _context = dbContext;

		public async Task<IActionResult> AssortmentList()
		{
			var controllers = new ProductRepository(_context);
			var categories = await controllers.GetLinkedList();
			var viewModel = new AssortmentListViewModel(categories);

			return View(viewModel);
		}
	}
}
