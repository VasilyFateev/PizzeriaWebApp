using DatabaseAccess;
using Microsoft.EntityFrameworkCore;
using ModelClasses;

namespace AdminApp.DatabaseControllers
{
    public class DatabaseController(AssortementSetupApplicationContext db)
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

                if (changes.ProductConfigurationChanges != null)
                    await ApllyTableChanges(changes.ProductConfigurationChanges);


                await db.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task ApllyTableChanges(TableChanges<ProductCategory> changes)
        {
            var removeList = changes.ToRemove;
            await db.ProductCategories.Where(category => removeList.Contains(category)).ExecuteDeleteAsync();

            foreach (var update in changes.ToUpdate)
                db.Entry(update).State = EntityState.Modified;

            var addList = changes.ToAdd.Where(c => db.ProductCategories.FirstOrDefault(x => x.Name == c.Name) == null);
            await db.ProductCategories.AddRangeAsync(addList);
        }

        private async Task ApllyTableChanges(TableChanges<Product> changes)
        {
            var removeList = changes.ToRemove;
            await db.Products.Where(product => removeList.Contains(product)).ExecuteDeleteAsync();

            foreach (var update in changes.ToUpdate)
                db.Entry(update).State = EntityState.Modified;

            var addList = changes.ToAdd.Where(c => db.ProductCategories.FirstOrDefault(x => x.Name == c.Name) == null);
            await db.Products.AddRangeAsync(addList);
        }

        private async Task ApllyTableChanges(TableChanges<ProductItem> changes)
        {
            var removeList = changes.ToRemove;
            await db.ProductItems.Where(item => removeList.Contains(item)).ExecuteDeleteAsync();

            foreach (var update in changes.ToUpdate)
                db.Entry(update).State = EntityState.Modified;

            await db.ProductItems.AddRangeAsync(changes.ToAdd);
        }

        private async Task ApllyTableChanges(TableChanges<Variation> changes)
        {
            var removeList = changes.ToRemove;
            await db.Variation.Where(variation => removeList.Contains(variation)).ExecuteDeleteAsync();

            foreach (var update in changes.ToUpdate)
                db.Entry(update).State = EntityState.Modified;

            var addList = changes.ToAdd.Where(c => db.Variation.FirstOrDefault(x => x.Name == c.Name) == null);
            await db.Variation.AddRangeAsync(addList);
        }
        private async Task ApllyTableChanges(TableChanges<VariationOption> changes)
        {
            var removeList = changes.ToRemove;
            await db.VariationOption.Where(option => removeList.Contains(option)).ExecuteDeleteAsync();

            foreach (var update in changes.ToUpdate)
                db.Entry(update).State = EntityState.Modified;

            var addList = changes.ToAdd.Where(c => db.VariationOption.FirstOrDefault(x => x.Name == c.Name) == null);
            await db.VariationOption.AddRangeAsync(addList);
        }

        private async Task ApllyTableChanges(TableChanges<ProductConfiguration> changes)
        {
            var removeList = changes.ToRemove;
            await db.ProductConfiguration.Where(configuration => removeList.Contains(configuration)).ExecuteDeleteAsync();

            foreach (var update in changes.ToUpdate)
                db.Entry(update).State = EntityState.Modified;

            await db.ProductConfiguration.AddRangeAsync(changes.ToAdd);
        }
    }
}