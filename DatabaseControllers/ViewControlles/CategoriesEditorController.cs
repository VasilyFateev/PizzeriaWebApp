using AdminApp.DatabaseControllers;
using AdminApp.CustomExceptions;
using ModelClasses;

namespace AdminApp.ViewController
{
    public class CategoriesEditorController
    {
        private readonly ProductCategory? category;
        private readonly TableChanges<ProductCategory> changes;
        private readonly IEnumerable<ProductCategory> categories;
        public CategoriesEditorController(IEnumerable<ProductCategory> categories, int categoryId, TableChanges<ProductCategory> changes)
        {
            if (categoryId > 0)
            {
                category = categories
                    .FirstOrDefault(p => p.Id == categoryId)
                    ?? throw new NotFoundEntityByKeyException(categoryId, typeof(ProductCategory));
            }
            
            this.changes = changes;
            this.categories = categories;
        }
        public void NameEdit(string newValue)
        {
            if (category != null)
            {
                CheckUniquenessName(newValue);
                category.Name = newValue;
                changes.ToUpdate.Add(category);
            }
            
        }
        public void Add(string name)
        {
            CheckUniquenessName(name);
            ProductCategory newCategory = new() { Name = name };
            changes.ToAdd.Add(newCategory);
        }

        public void Remove()
        {
            if (category != null)
            {
                changes.ToRemove.Add(category);
            }
        }

        private void CheckUniquenessName(string name)
        {
            if (categories.Any(c => c.Name == name))
            {
                throw new NonUniqueNameException(typeof(ProductCategory));
            }
        }
    }
}