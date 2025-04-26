using AdminApp.DatabaseControllers;
using AdminApp.CustomExceptions;
using ModelClasses;

namespace AdminApp.ViewController
{
    public class VariationEditorController
    {
        private readonly Variation? variation;
        private readonly TableChanges<Variation> changes;
        private readonly List<ProductCategory> categories;
        public VariationEditorController(List<ProductCategory> categories, int varuationId, TableChanges<Variation> changes)
        {
            if (varuationId > 0)
            {
                variation = categories
                    .SelectMany(c => c.Variations)
                    .FirstOrDefault(v => v.Id == varuationId)
                    ?? throw new NotFoundEntityByKeyException(varuationId, typeof(Variation));
            }
            
            this.changes = changes;
            changes ??= new();
            this.categories = categories;
        }
        public void NameEdit(string newValue)
        {
            if (variation != null)
            {
                CheckUniquenessName(newValue);

                variation.Name = newValue;
                changes.ToUpdate ??= [];
                changes.ToUpdate.Add(variation);
            }
        }

        public void Add(int categoryId, string name)
        {
            var category = categories
                .FirstOrDefault(c => c.Id == categoryId)
                ?? throw new NotFoundEntityByKeyException(categoryId, typeof(ProductCategory));

            CheckUniquenessName(name);
            changes.ToAdd ??= [];
            Variation newVariation = new() { CategoryId = category.Id, Name = name};
            changes.ToAdd.Add(newVariation);
        }

        public void Remove()
        {
            if (variation != null)
            {
                changes.ToRemove ??= [];
                changes.ToRemove.Add(variation);
            }
        }

        private void CheckUniquenessName(string name)
        {
            if (categories.SelectMany(c => c.Variations).Any(v => v.Name == name))
            {
                throw new NonUniqueNameException(typeof(Variation));
            }
        }
    }
}