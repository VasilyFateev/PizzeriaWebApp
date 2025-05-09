using AssortmentEditService.DatabaseControllers;
using AssortmentEditService.CustomExceptions;
using ModelClasses;

namespace AssortmentEditService.ViewController
{
    public class ProductItemEditorController
    {
        private readonly ProductItem? productItem;
        private readonly TableChanges<ProductItem> changes;
        private readonly IEnumerable<ProductCategory> categories;
        public ProductItemEditorController(IEnumerable<ProductCategory> categories, int productItemId, TableChanges<ProductItem> changes)
        {
            if (productItemId > 0)
            {
                productItem = categories.SelectMany(c => c.Products)
                    .SelectMany(p => p.ProductItems)
                    .FirstOrDefault(p => p.Id == productItemId)
                    ?? throw new NotFoundEntityByKeyException(productItemId, typeof(ProductItem));
            }
            this.changes = changes;
            this.categories = categories;
        }

        public void PriceEdit(int newValue)
        {
            if (productItem != null)
            {
                productItem.Price = newValue;
                changes.ToUpdate.Add(productItem);
            }           
        }

        public void CookingTimeEdit(int newValue)
        {
            if (productItem != null)
            {
                productItem.CookingTime = newValue;
                changes.ToUpdate.Add(productItem);
            }

        }
        public void Add(int productId, int price = 0, int cookingTime = 0)
        {
            var product = categories
                .SelectMany(c => c.Products)
                .FirstOrDefault(p => p.Id == productId)
                ?? throw new NotFoundEntityByKeyException(productId, typeof(Product));

            ProductItem newProductItem = new() { ProductId = product.Id, Price = price, CookingTime = cookingTime };
            changes.ToAdd.Add(newProductItem);
        }

        public void Remove()
        {
            if (productItem != null)
            {
                changes.ToRemove.Add(productItem);
            }
        }
    }
}