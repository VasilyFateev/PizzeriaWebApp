using DatabaseAccess;
using Microsoft.EntityFrameworkCore;
using ModelClasses;

namespace DatabaseControllers
{
    public partial class ProductRepository
    {
        private readonly AssortementSetupApplicationContext db;

        public ProductRepository()
        {
            db = new AssortementSetupApplicationContext();
        }

        public async Task<List<ProductCategory>> GetProductList()
        {
            var categories = await db.ProductCategories
                .Include(c => c.Products)
                    .ThenInclude(p => p.ProductItems)
                .Include(c => c.Variations)
                    .ThenInclude(v => v.VariationOptions)
                .AsNoTracking()
                .ToListAsync();
            return categories;
        }

        public async Task UpdateAssortimentData(AssortimentDatabaseChanges changes)
        {
            await using var transaction = await db.Database.BeginTransactionAsync();
            try
            {
                if (changes.CategoriesChanges != null)                
                    await ApllyTableChanges(changes.CategoriesChanges);

                if (changes.ProductChanges != null)
                    await ApllyTableChanges(changes.ProductChanges);

                if (changes.ProductItemChanges != null)
                    await ApllyTableChanges(changes.ProductItemChanges);

                if (changes.VariationChanges != null)
                    await ApllyTableChanges(changes.VariationChanges);

                if (changes.VariationOptionChanges != null)
                    await ApllyTableChanges(changes.VariationOptionChanges);


                await db.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task ApllyTableChanges(TableChanges<ProductCategory> changes)
        {
            var removeList = changes.ToRemove;
            await db.ProductCategories.Where(c => removeList.Contains(c)).ExecuteDeleteAsync();

            foreach (var update in changes.ToUpdate)
                db.Entry(update).State = EntityState.Modified;

            var addList = changes.ToAdd.Where(c => db.ProductCategories.FirstOrDefault(x => x.Name == c.Name) == null);
            await db.ProductCategories.AddRangeAsync(addList);
        }

        private async Task ApllyTableChanges(TableChanges<Product> changes)
        {
            var removeList = changes.ToRemove;
            await db.Products.ForEachAsync(c => c.CategoryId = default);

            foreach (var update in changes.ToUpdate)
                db.Entry(update).State = EntityState.Modified;

            var addList = changes.ToAdd.Where(c => db.ProductCategories.FirstOrDefault(x => x.Name == c.Name) == null);
            await db.Products.AddRangeAsync(addList);
        }

        private async Task ApllyTableChanges(TableChanges<ProductItem> changes)
        {
            var removeList = changes.ToRemove;
            await db.ProductItems.ForEachAsync(c => c.ProductId = default);

            foreach (var update in changes.ToUpdate)
                db.Entry(update).State = EntityState.Modified;

            await db.ProductItems.AddRangeAsync(changes.ToAdd);
        }

        private async Task ApllyTableChanges(TableChanges<Variation> changes)
        {
            var removeList = changes.ToRemove;
            await db.Variation.ForEachAsync(c => c.CategoryId = default);

            foreach (var update in changes.ToUpdate)
                db.Entry(update).State = EntityState.Modified;

            var addList = changes.ToAdd.Where(c => db.Variation.FirstOrDefault(x => x.Name == c.Name) == null);
            await db.Variation.AddRangeAsync(addList);
        }
        private async Task ApllyTableChanges(TableChanges<VariationOption> changes)
        {
            var removeList = changes.ToRemove;
            await db.VariationOption.ForEachAsync(c => c.VariationId = default);

            foreach (var update in changes.ToUpdate)
                db.Entry(update).State = EntityState.Modified;

            var addList = changes.ToAdd.Where(c => db.VariationOption.FirstOrDefault(x => x.Name == c.Name) == null);
            await db.VariationOption.AddRangeAsync(addList);
        }

        private async Task ApllyTableChanges(TableChanges<ProductConfiguration> changes)
        {
            var removeList = changes.ToRemove;
            await db.ProductConfiguration.ForEachAsync(c => c.ProductItemId = default);

            foreach (var update in changes.ToUpdate)
                db.Entry(update).State = EntityState.Modified;

            await db.ProductConfiguration.AddRangeAsync(changes.ToAdd);
        }
    }
}