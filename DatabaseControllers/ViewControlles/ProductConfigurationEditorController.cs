using AssortmentEditService.DatabaseControllers;
using AssortmentEditService.CustomExceptions;
using ModelClasses;

namespace AssortmentEditService.ViewController
{
    public class ProductConfigurationEditorController
    {
        private readonly ProductConfiguration? configuration;
        private readonly TableChanges<ProductConfiguration> changes;
        private readonly IEnumerable<ProductCategory> categories;
        public ProductConfigurationEditorController(IEnumerable<ProductCategory> categories, int productItemId, int variationOptionId, TableChanges<ProductConfiguration> changes)
        {
            if (productItemId > 0 && variationOptionId > 0)
            {
                configuration = categories.SelectMany(c => c.Products)
                    .SelectMany(p=>p.ProductItems)
                    .SelectMany(pi=>pi.Configurations)
                    .FirstOrDefault(conf => conf.ProductItemId == productItemId && conf.VariationOptionId == variationOptionId)
                    ?? throw new NotFoundEntityByCompositeKeyException(productItemId, variationOptionId, typeof(ProductConfiguration));
            }

            this.changes = changes;
            this.categories = categories;
        }

        public void Add(int productItemId, int variationOptionId)
        {
            var productItem = categories
                .SelectMany(c=>c.Products)
                .SelectMany(p=>p.ProductItems)
                .FirstOrDefault(pi => pi.Id == productItemId) 
                ?? throw new NotFoundEntityByKeyException(productItemId, typeof(ProductItem));

            var variationOption = categories
                .SelectMany(c => c.Variations)
                .SelectMany(v => v.VariationOptions)
                .FirstOrDefault(vo => vo.Id == variationOptionId) 
                ?? throw new NotFoundEntityByKeyException(variationOptionId, typeof(VariationOption));

            ProductConfiguration newConfiguration = new() { ProductItemId = productItem.Id, VariationOptionId = variationOption.Id };
            changes.ToAdd.Add(newConfiguration);
        }

        public void Remove()
        {
            if (configuration != null)
            {
                changes.ToRemove.Add(configuration);
            }
        }
    }
}