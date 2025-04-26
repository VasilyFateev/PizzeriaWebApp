using Microsoft.EntityFrameworkCore;
using ModelClasses;

namespace DatabaseControllers
{
    public class AssortimentDatabaseChanges
    {
        public TableChanges<ProductCategory>? CategoriesChanges { get; set; }
        public TableChanges<Product>? ProductChanges { get; set; }
        public TableChanges<ProductItem>? ProductItemChanges { get; set; }
        public TableChanges<Variation>? VariationChanges { get; set; }
        public TableChanges<VariationOption>? VariationOptionChanges { get; set; }
    }

    public class TableChanges<T> where T : class
    {
        public List<T> ToAdd { get; set; } = [];
        public List<T> ToRemove { get; set; } = [];
        public List<T> ToUpdate { get; set; } = [];
    }
}