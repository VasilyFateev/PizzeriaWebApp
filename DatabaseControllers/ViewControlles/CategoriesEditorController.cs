using AdminApp.DatabaseControllers;
using AdminApp.CustomExceptions;
using ModelClasses;

namespace AdminApp.ViewController
{
    public class CategoriesEditorController
    {
        private readonly ProductCategory? category;
        private readonly TableChanges<ProductCategory> changes;
        private readonly List<ProductCategory> categories;
        public CategoriesEditorController(List<ProductCategory> categories, int categoryId, TableChanges<ProductCategory> changes)
        {
            if (categoryId > 0)
            {
                category = categories
                    .FirstOrDefault(p => p.Id == categoryId)
                    ?? throw new NotFoundEntityByKeyException(categoryId, typeof(ProductCategory));
            }
            
            this.changes = changes;
            changes ??= new();
            this.categories = categories;
        }
        public void NameEdit(string newValue)
        {
            if (category != null)
            {
                CheckUniquenessName(newValue);
                category.Name = newValue;
                changes.ToUpdate ??= [];
                changes.ToUpdate.Add(category);
            }
            
        }
        public void Add(string name)
        {
            CheckUniquenessName(name);
            changes.ToAdd ??= [];
            ProductCategory newCategory = new() { Name = name };
            changes.ToAdd.Add(newCategory);
        }

        public void Remove()
        {
            if (category != null)
            {
                changes.ToRemove ??= [];
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