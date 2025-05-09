using DatabaseAccess;
using Microsoft.EntityFrameworkCore;
using ModelClasses;
using ModelClasses.Interfaces;

namespace AdminApp.DatabaseControllers
{
	public class DatabaseController(AssortementDataContext db)
	{
		public async Task<List<ProductCategory>> GetLinkedList()
		{
			var categories = await db.ProductCategories
				.Include(c => c.Products)
					.ThenInclude(p => p.ProductItems)
						.ThenInclude(pi => pi.Configurations)
				.Include(c => c.Variations)
					.ThenInclude(v => v.VariationOptions)
						.ThenInclude(pi => pi.Configurations)
				.AsNoTracking()
				.ToListAsync();
			return categories;
		}
		public async Task<Product?> GetProduct(int id)
		{
			Product? product = await db.Products
				.Where(product => product.Id == id)
				.Include(product => product.Category)
					.ThenInclude(category => category.Variations)
						.ThenInclude(variation => variation.VariationOptions)
				.Include(product => product.ProductItems)
					.ThenInclude(item => item.Configurations)
				.FirstOrDefaultAsync();
			return product;
		}

		public async Task<ProductCategory?> GetCategory(int id)
		{
			ProductCategory? category = await db.ProductCategories
				.Where(category => category.Id == id)
				.Include(category => category.Products)
				.Include(category => category.Variations)
					.ThenInclude(variation => variation.VariationOptions)
				.FirstOrDefaultAsync();
			return category;
		}

		public async Task UpdateAssortimentData(AssortimentDatabaseChanges changes)
		{
			await using var transaction = await db.Database.BeginTransactionAsync();
			try
			{
				if (changes.CategoriesChanges != null)
				{
					await ApplyTableChanges(changes.CategoriesChanges, db.ProductCategories);
					await db.SaveChangesAsync();
				}

				if (changes.ProductChanges != null)
				{
					await ApplyTableChanges(changes.ProductChanges, db.Products);
					await db.SaveChangesAsync();
				}

				if (changes.ProductItemChanges != null)
				{
					await ApplyTableChanges(changes.ProductItemChanges, db.ProductItems);
					await db.SaveChangesAsync();
				}

				if (changes.VariationChanges != null)
				{
					await ApplyTableChanges(changes.VariationChanges, db.Variation);
					await db.SaveChangesAsync();
				}

				if (changes.VariationOptionChanges != null)
				{
					await ApplyTableChanges(changes.VariationOptionChanges, db.VariationOption);
					await db.SaveChangesAsync();
				}

				if (changes.ProductConfigurationChanges != null)
					await ApplyTableChanges(changes.ProductConfigurationChanges);

				await db.SaveChangesAsync();
				await transaction.CommitAsync();
			}
			catch (Exception)
			{
				await transaction.RollbackAsync();
				throw;
			}
		}
		private async Task ApplyTableChanges<T>(TableChanges<T> changes, DbSet<T> dbSet) where T : class, IHasPrimaryKey
		{
			var removeList = changes.ToRemove;
			await dbSet.Where(category => removeList.Contains(category)).ExecuteDeleteAsync();

			for (int i = 0; i < changes.ToUpdate.Count; i++)
			{
				var updatedItem = changes.ToUpdate[i];
				if (updatedItem == null) continue;

				var existingItem = dbSet.Find(updatedItem.GetPrimaryKey());
				if (existingItem != null)
				{
					db.Entry(existingItem).CurrentValues.SetValues(updatedItem);
				}
			}

			var addList = changes.ToAdd
				.Where(c => c is not IHasUniqueName CategoryWithUniqueName ||
					dbSet.FirstOrDefault(x => (x as IHasUniqueName)
					.GetUniqueName() == CategoryWithUniqueName.GetUniqueName()) == null);

			await dbSet.AddRangeAsync(changes.ToAdd);
		}

		private async Task ApplyTableChanges(TableChanges<ProductConfiguration> changes)
		{
			if (changes.ToRemove.Count > 0)
			{
				var query = db.ProductConfiguration.AsQueryable();
				foreach (var item in changes.ToRemove.Where(x => x != null))
				{
					query = query.Where(x => x.ProductItemId == item.ProductItemId && x.VariationOptionId == item.VariationOptionId);
				}
				await query.ExecuteDeleteAsync();
			}

			foreach (var updatedItem in changes.ToUpdate.Where(x => x != null))
			{
				var existingItem = await db.ProductConfiguration
					.FirstOrDefaultAsync(x => x.ProductItemId == updatedItem.ProductItemId && x.VariationOptionId == updatedItem.VariationOptionId);

				if (existingItem != null)
				{
					db.Entry(existingItem).CurrentValues.SetValues(updatedItem);
				}
			}

			if (changes.ToAdd.Count > 0)
			{
				var validToAdd = new List<ProductConfiguration>();
				foreach (var item in changes.ToAdd)
				{
					var productItemExists = await db.ProductItems.AnyAsync(x => x.Id == item.ProductItemId);
					var optionExists = await db.VariationOption.AnyAsync(x => x.Id == item.VariationOptionId);

					if (productItemExists && optionExists)
					{
						validToAdd.Add(item);
					}
				}

				await db.ProductConfiguration.AddRangeAsync(validToAdd);
			}
		}
	}
}