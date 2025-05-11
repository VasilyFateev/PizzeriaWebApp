using AssortmentDatabaseAccess;
using AssortmentEditService.CustomExceptions;
using Microsoft.EntityFrameworkCore;
using ProductModelClasses;

namespace OrderOrchestratorService
{
    public class AssortmentPresentationController(AssortementDataContext db)
    {
        public async Task<List<ProductCategory>> GetAssortmentList()
        {
            return await db.ProductCategories
                .Include(c => c.Products)
                    .ThenInclude(p => p.ProductItems)
                .ToListAsync();
        }

        public async Task<Product> GetProductAllInfo(int id)
        {
            var product = await db.Products
                .Where(p => p.Id == id)
                .Include(p => p.ProductItems)
                    .ThenInclude(pi => pi.Configurations)
                .Include(p => p.Category)
                    .ThenInclude(c => c.Variations)
                        .ThenInclude(v => v.VariationOptions)
                            .ThenInclude(vo => vo.Configurations)
                            .FirstOrDefaultAsync()
                 ?? throw new NotFoundEntityByKeyException(id, typeof(Product));
            return product;

        }
    }
}
