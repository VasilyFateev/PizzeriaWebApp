using AdminApp.DatabaseControllers;
using AdminAppView.Models;
using DatabaseAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelClasses;

namespace AdminAppView.Controllers
{
	public class AssortmentController : Controller
	{
		private readonly AssortementSetupApplicationContext _context;
		private readonly DatabaseController databaseController;
		private readonly AssortimentDatabaseChanges databaseChanges;

		public AssortmentController(AssortementSetupApplicationContext dbContext)
		{
			_context = dbContext;
			databaseController = new DatabaseController(_context);
			databaseChanges = new();
		}

		public async Task<IActionResult> AssortmentList()
		{
			var controllers = new DatabaseController(_context);
			var categories = await controllers.GetLinkedList();
			var viewModel = new AssortmentListViewModel(categories);

			return View(viewModel);
		}

		public async Task<IActionResult> ProductEditPanel(int id)
		{
			var controllers = new DatabaseController(_context);
			var categories = await controllers.GetLinkedList();
			var product = categories.SelectMany(c => c.Products).FirstOrDefault(p => p.Id == id);
			if (product != null)
			{
				var viewModel = new ProductEditorViewModel(product);
				return View(viewModel);
			}
			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> AddProduct(int categoryId, ProductViewModel model)
		{
			var category = await _context.ProductCategories.FirstOrDefaultAsync(category => category.Id == categoryId);
			if (category == null)
			{
				return NotFound("Category is not found");
			}

			if(_context.Products.Any(p => p.Name == model.Name))
			{
				return BadRequest("The name value is not unique");
			}

			category.Products.Add(
					new Product()
					{

					});
			await _context.SaveChangesAsync();

			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> EditProduct(ProductEditorViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View("ProductEditPanel", model);
			}

			try
			{
				var product = await _context.Products
					.Include(p => p.ProductItems)
					.ThenInclude(pi => pi.Configurations)
					.FirstOrDefaultAsync(p => p.Id == model.Product.Id);

				if (product == null)
				{
					return NotFound();
				}

				product.Name = model.Product.Name;

				await _context.SaveChangesAsync();
				return RedirectToAction("AssortmentList");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", "Ошибка при сохранении: " + ex.Message);
				return View("ProductEditPanel", model);
			}
		}

		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
		{
			var productToRemove = await _context.Products.FirstOrDefaultAsync(product => product.Id == id);
			if (productToRemove == null)
			{
				return NotFound("Product is not found");
			}
			_context.Products.Remove(productToRemove);
			await _context.SaveChangesAsync();
			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> AddProductItem(int productId, ProductItemViewModel model)
		{
			var product = await _context.Products.FirstOrDefaultAsync(product => product.Id == productId);
			if (product == null)
			{
				return NotFound("Product is not found");
			}

			var optionIds = model.Options.Select(o => o.Id).ToList();
			var existingOptionsId = await _context.VariationOption
				.Where(vo => optionIds.Contains(vo.Id))
				.Select(vo => vo.Id)
				.ToListAsync();
			var missingIds = optionIds.Except(existingOptionsId).ToList();

			if (missingIds.Count != 0)
			{
				return NotFound($"Variation options not found: {string.Join(", ", missingIds)}");
			}

			product.ProductItems.Add(
					new ProductItem()
					{
						ProductId = productId,
						Price = model.Price,
						Configurations = existingOptionsId.Select(optionId => new ProductConfiguration
						{
							VariationOptionId = optionId
						}).ToList()
					});
			await _context.SaveChangesAsync();

			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> EditProductItem(ProductEditorViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View("ProductEditPanel", model);
			}

			try
			{
				var product = await _context.Products
					.Include(p => p.ProductItems)
					.ThenInclude(pi => pi.Configurations)
					.FirstOrDefaultAsync(p => p.Id == model.Product.Id);

				if (product == null)
				{
					return NotFound();
				}

				product.Name = model.Product.Name;

				await _context.SaveChangesAsync();
				return RedirectToAction("AssortmentList");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", "Ошибка при сохранении: " + ex.Message);
				return View("ProductEditPanel", model);
			}
		}

		[HttpPost]
		public async Task<IActionResult> DeleteProductItem(int id)
		{
			var productItemToRemove = await _context.ProductItems.FirstOrDefaultAsync(item => item.Id == id);
			if (productItemToRemove == null)
			{
				return NotFound("Product is not found");
			}
			_context.ProductItems.Remove(productItemToRemove);
			await _context.SaveChangesAsync();

			return Ok();
		}

		
		
	}
}
