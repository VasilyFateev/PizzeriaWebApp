using ProductModelClasses;

namespace AssortmentEditService.DatabaseControllers
{
    public class AssortimentDatabaseChanges
    {
        public AssortimentDatabaseChanges()
        {
            CategoriesChanges = new();
            ProductChanges = new();
            ProductItemChanges = new();
            VariationChanges = new();
            VariationOptionChanges = new();
            ProductConfigurationChanges = new();
        }

        public TableChanges<ProductCategory> CategoriesChanges { get; set; }
        public TableChanges<Product> ProductChanges { get; set; }
        public TableChanges<ProductItem> ProductItemChanges { get; set; }
        public TableChanges<Variation> VariationChanges { get; set; }
        public TableChanges<VariationOption> VariationOptionChanges { get; set; }
        public TableChanges<ProductConfiguration> ProductConfigurationChanges { get; set; }
    }
}