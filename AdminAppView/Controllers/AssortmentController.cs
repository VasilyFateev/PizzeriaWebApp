using AdminApp.DatabaseControllers;
using AdminAppView.Models;
using DatabaseAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelClasses;
using System.Collections.Generic;

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
			var viewModel = new AssortmentRequestVM()
			{
				Categories = categories.Select(category => new AssortmentRequestVM.CategoryVM()
				{
					Id = category.Id,
					Name = category.Name,
					Products = category.Products.Select(product => new AssortmentRequestVM.ProductVM()
					{
						Id = product.Id,
						Name = product.Name,
						Variations = category.Variations.Select(variation => new AssortmentRequestVM.VariationVM()
						{
							Id = variation.Id,
							Name = variation.Name,
							Options = variation.VariationOptions
							.Select(option => new AssortmentRequestVM.VariationOptionVM()
							{
								Id = option.Id,
								Name = option.Name,
								Status = option.Configurations
								.Select(conf => conf.ProductItemId)
								.Any(itemId => product.ProductItems
									.Any(item => item.Id == itemId))
							}).ToArray()
						}).ToArray()
					}).ToArray()
				}).ToArray()
			};

			return View(viewModel);
		}

		public async Task<IActionResult> CategoryEditPanel(int id)
		{
			var controllers = new DatabaseController(_context);
			ProductCategory? category = await controllers.GetCategory(id);
			if (category == null)
			{
				return NotFound();
			}

			var viewModel = new CategoryEditRequestResponceVM
			{
				Id = category.Id,
				Name = category.Name,
				Products = category.Products
				.Select(product => new CategoryEditRequestResponceVM.ProductVM()
				{
					Id = product.Id,
					Name = product.Name
				}).ToArray(),
				Variations = category.Variations
				.Select(variation => new CategoryEditRequestResponceVM.VariationVM()
				{
					Id = variation.Id,
					Name = variation.Name,
					Options = variation.VariationOptions
					.Select(option => new CategoryEditRequestResponceVM.VariationOptionVM()
					{
						Id = option.Id,
						Name = option.Name,
					}).ToArray()
				}).ToArray()
			};
			return View(viewModel);
		}

		public async Task<IActionResult> ProductEditPanel(int id)
		{
			var controllers = new DatabaseController(_context);
			Product? product = await controllers.GetProduct(id);
			if (product == null)
			{
				return NotFound();
			}

			var viewModel = new ProductEditRequestVM
			{
				Id = product.Id,
				Name = product.Name,
				Description = product.Description,
				ProductItems = product.ProductItems
				.Select(item => new ProductEditRequestVM.ProductItemVM()
				{
					Id = item.Id,
					Price = item.Price,
					CookingTime = item.CookingTime,
					Configurations = item.Configurations
					.Select(conf => new ProductEditRequestVM.ConfigurationVM()
					{
						ItemId = item.Id,
						OptionId = conf.VariationOptionId
					}).ToArray(),
				}).ToArray(),
				CategoryVariations = product.Category.Variations
				.Select(variation => new ProductEditRequestVM.VariationVM()
				{
					Id = variation.Id,
					Name = variation.Name,
					VariationOptions = variation.VariationOptions
					.Select(option => new ProductEditRequestVM.VariationOptionVM()
					{
						Id = option.Id,
						Name = option.Name,
						VariationID = option.VariationId
					}).ToArray()
				}).ToArray()
			};

			return View(viewModel);
		}



		[HttpPost]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> UpdateProduct([FromBody] ProductEditResponceVM model)
		{
			if (!ModelState.IsValid)
			{
				var errors = ModelState.Values.SelectMany(v => v.Errors);
				return await ProductEditPanel(model.Id);
			}

			var categories = await databaseController.GetLinkedList();

			var productList = categories.SelectMany(c => c.Products).ToList();
			var product = productList.FirstOrDefault(p => p.Id == model.Id);
			if (product == null)
			{
				return NotFound();
			}

			if (productList.Any(p => p.Name == model.Name && p.Id != model.Id))
			{
				return await ProductEditPanel(model.Id);
			}


			product.Id = model.Id;
			product.Name = model.Name;
			product.Description = model.Description;
			var modelItems = model.ProductItems;
			var allProductItems = categories.SelectMany(c => c.Products).SelectMany(p => p.ProductItems);

			var itemsToRemoveId = allProductItems
				.Where(i => i.ProductId == model.Id)
				.Select(i => i.Id)
				.Except(modelItems
					.Select(mi => mi.Id));
			databaseChanges.ProductItemChanges.ToRemove.AddRange(allProductItems.Where(pi => itemsToRemoveId.Contains(pi.Id)));

			foreach (var itemVm in modelItems)
			{
				var productItem = allProductItems.FirstOrDefault(pi => pi.Id == itemVm.Id);

				var newProductItem = new ProductItem()
				{
					Id = itemVm.Id,
					CookingTime = itemVm.CookingTime,
					ProductId = model.Id,
					Price = itemVm.Price,
					Configurations = []
				};

				if (productItem == null)
				{
					foreach (var configuration in itemVm.Configurations)
					{
						var newConfiguration = new ProductConfiguration()
						{
							ProductItemId = configuration.ItemId,
							VariationOptionId = configuration.OptionId
						};
						databaseChanges.ProductConfigurationChanges.ToAdd.Add(newConfiguration);
					}
					databaseChanges.ProductItemChanges.ToAdd.Add(newProductItem);
				}
				else
				{
					databaseChanges.ProductConfigurationChanges.ToRemove.AddRange(productItem.Configurations
						.Where(piConf => !itemVm.Configurations
							.Any(modelConf => modelConf.OptionId == piConf.VariationOptionId))
						);


					databaseChanges.ProductConfigurationChanges.ToAdd.AddRange(itemVm.Configurations
						.Where(configuration => !productItem.Configurations.Any(conf => conf.VariationOptionId == configuration.OptionId))
						.Select(configuration => new ProductConfiguration()
						{
							ProductItemId = configuration.ItemId,
							VariationOptionId = configuration.OptionId
						}));

					databaseChanges.ProductItemChanges.ToUpdate.Add(newProductItem);
				}
			}
			databaseChanges.ProductChanges.ToUpdate.Add(product);
			await databaseController.UpdateAssortimentData(databaseChanges);
			return Ok();
		}

		[HttpPost]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> UpdateCategory([FromBody] CategoryEditRequestResponceVM model)
		{
			if (!ModelState.IsValid)
			{
				var errors = ModelState.Values.SelectMany(v => v.Errors);
				return await CategoryEditPanel(model.Id);
			}

			var categories = await databaseController.GetLinkedList();
			var category = categories.FirstOrDefault(c => c.Id == model.Id);
			if (category == null)
			{
				return NotFound();
			}

			if (categories.Any(p => p.Name == model.Name && p.Id != model.Id))
			{
				return await CategoryEditPanel(model.Id);
			}

			var dbCategory = await databaseController.GetCategory(model.Id);
			if (dbCategory == null)
			{
				return NotFound();
			}

			category.Id = model.Id;
			category.Name = model.Name;
			databaseChanges.CategoriesChanges.ToUpdate.Add(category);

			var productsVm = model.Products;
			var variationsVm = model.Variations;
			var optionsVm = variationsVm.SelectMany(v => v.Options).AsQueryable();

			var dbVariations = category.Variations.AsQueryable();

			var removedVariations = category.Variations
				.ExceptBy(variationsVm
				.Select(v => v.Id), v => v.Id);
			databaseChanges.VariationChanges.ToRemove.AddRange(removedVariations);

			var newVariations = variationsVm
				.ExceptBy(category.Variations
				.Select(v => v.Id), v => v.Id)
				.Select(v => new Variation()
				{
					Name = v.Name,
					CategoryId = category.Id,
					VariationOptions = v.Options
						.Select(o => new VariationOption()
						{
							Id = o.Id,
							Name = o.Name,
							VariationId = v.Id
						}).ToArray()
				});
			databaseChanges.VariationChanges.ToAdd.AddRange(newVariations);

			var updateableVariations = variationsVm
				.ExceptBy(newVariations
				.Select(v => v.Id), v => v.Id);
			databaseChanges.VariationChanges.ToUpdate
				.AddRange(updateableVariations
					.Where(v => v.Name != dbVariations
						.First(dbVar => dbVar.Id == v.Id).Name)
					.Select(v => new Variation()
					{
						Id = v.Id,
						Name = v.Name,
						CategoryId = category.Id
					}));

			foreach (var variation in updateableVariations)
			{
				var dbOptions = dbVariations.First(v => v.Id == variation.Id).VariationOptions.AsQueryable();
				var variationOptions = variation.Options.AsQueryable();
				var removedOptions = dbOptions
					.ExceptBy(variationOptions
					.Select(o => o.Id), o => o.Id);
				databaseChanges.VariationOptionChanges.ToRemove.AddRange(removedOptions);

				var newOptions = variationOptions
					.ExceptBy(dbOptions
					.Select(o => o.Id), o => o.Id)
					.Select(o => new VariationOption()
					{
						Name = o.Name,
						VariationId = variation.Id
					});
				databaseChanges.VariationOptionChanges.ToAdd.AddRange(newOptions);

				var updateableOptions = variationOptions
					.ExceptBy(newOptions
					.Select(o => o.Id), o => o.Id)
					.Where(o => o.Name != dbOptions
						.First(o1 => o1.Id == o.Id).Name)
					.Select(o => new VariationOption()
					{
						Id = o.Id,
						Name = o.Name,
						VariationId = variation.Id,
					});
				databaseChanges.VariationOptionChanges.ToUpdate.AddRange(updateableOptions);
			}

			var removedProduct = category.Products
				.ExceptBy(productsVm
				.Select(p => p.Id), p => p.Id);
			databaseChanges.ProductChanges.ToRemove.AddRange(removedProduct);

			var newProducts = productsVm
				.ExceptBy(category.Products
				.Select(p => p.Id), p => p.Id)
				.Select(p => new Product()
				{
					Name = p.Name,
					CategoryId = category.Id,
					Description = "N/A",
					ImageLink = "N/A",
				});
			databaseChanges.ProductChanges.ToAdd.AddRange(newProducts);

			await databaseController.UpdateAssortimentData(databaseChanges);
			return Ok();
		}
	}
}
