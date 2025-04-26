using AdminApp.DatabaseControllers;
using AdminApp.CustomExceptions;
using ModelClasses;

namespace AdminApp.ViewController
{
    public class ProductEditorController
    {
        private readonly Product? product;
        private readonly TableChanges<Product> changes;
        private readonly List<ProductCategory> categories;
        public ProductEditorController(List<ProductCategory> categories, int productId, TableChanges<Product> changes)
        {
            if (productId > 0)
            {
                product = categories
                    .SelectMany(c => c.Products)
                    .FirstOrDefault(p => p.Id == productId)
                    ?? throw new NotFoundEntityByKeyException(productId, typeof(Product));
            }
            this.changes = changes;
            changes ??= new();
            this.categories = categories;
        }
        public void NameEdit(string newValue)
        {
            if (product != null)
            {
                CheckUniquenessName(newValue);
                product.Name = newValue;
                changes.ToUpdate ??= [];
                changes.ToUpdate.Add(product);
            }
        }

        public void DescriptionEdit(string newValue)
        {
            if (product != null)
            {
                product.Description = newValue;
                changes.ToUpdate ??= [];
                changes.ToUpdate.Add(product);
            }            
        }
        public void Add(int categoryId, string name, string imageLink, string description = "")
        {
            var category = categories
                .FirstOrDefault(c => c.Id == categoryId)
                ?? throw new NotFoundEntityByKeyException(categoryId, typeof(ProductCategory));

            CheckUniquenessName(name);
            changes.ToAdd ??= [];
            Product newProduct = new() { CategoryId = category.Id, Name = name, ImageLink = imageLink, Description = description };
            changes.ToAdd.Add(newProduct);
        }

        public void Remove()
        {
            if (product != null)
            {
                changes.ToRemove ??= [];
                changes.ToRemove.Add(product);
            }
        }
        private void CheckUniquenessName(string name)
        {
            if (categories.SelectMany(c => c.Products).Any(p => p.Name == name))
            {
                throw new NonUniqueNameException(typeof(Product));
            }
        }
    }
}