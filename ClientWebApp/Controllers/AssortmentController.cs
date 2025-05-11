using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AssortmentDatabaseAccess;
using OrderOrchestratorService;
using ClientWebApp.Models;

namespace ClientWebApp.Controllers
{
    public class AssortmentController : Controller
    {
        private readonly AssortementDataContext _context;
        private readonly AssortmentPresentationController databaseController;

        public AssortmentController(AssortementDataContext dbContext)
        {
            _context = dbContext;
            databaseController = new AssortmentPresentationController(_context);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Index()
        {
            var categories = await databaseController.GetAssortmentList();
            var viewModel = new AssortmentListRequestVM()
            {
                Categories = categories
                .Select(c => new AssortmentListRequestVM.CategoryVM()
                {
                    ID = c.Id,
                    Name = c.Name,
                    Products = c.Products
                    .Select(p => new AssortmentListRequestVM.ProductVM()
                    {
                        ID = p.Id,
                        Name = p.Name,
                        Description = p.Description ?? string.Empty,
                        MinPrice = p.ProductItems
                            .Select(pi => pi.Price)
                            .Min()
                    }).ToArray()
                }).ToArray()
            };
            return View(viewModel);
        }
    }
}
