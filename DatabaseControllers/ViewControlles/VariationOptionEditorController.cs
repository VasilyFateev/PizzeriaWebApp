using AssortmentEditService.DatabaseControllers;
using AssortmentEditService.CustomExceptions;
using ModelClasses;

namespace AssortmentEditService.ViewController
{
    public class VariationOptionEditorController
    {
        private readonly VariationOption? option;
        private readonly TableChanges<VariationOption> changes;
        private readonly IEnumerable<ProductCategory> categories;
        public VariationOptionEditorController(IEnumerable<ProductCategory> categories, int variationOptionId, TableChanges<VariationOption> changes)
        {
            if (variationOptionId > 0)
            {
                option = categories
                    .SelectMany(c => c.Variations)
                    .SelectMany(p => p.VariationOptions)
                    .FirstOrDefault(p => p.Id == variationOptionId)
                    ?? throw new NotFoundEntityByKeyException(variationOptionId, typeof(VariationOption));
            }
            this.changes = changes;
            this.categories = categories;
        }
        public void NameEdit(string newValue)
        {
            if (option != null)
            {
                CheckUniquenessName(newValue);

                option.Name = newValue;
                changes.ToUpdate.Add(option);
            }
        }

        public void Add(int variationId, string optionName)
        {
            var variation = categories
                .SelectMany(c => c.Variations)
                .FirstOrDefault(v => v.Id == variationId)
                ?? throw new NotFoundEntityByKeyException(variationId, typeof(Variation));
            changes.ToAdd ??= [];
            VariationOption newVariationOption = new() { VariationId = variation.Id, Name = optionName };
            changes.ToAdd.Add(newVariationOption);
        }

        public void Remove()
        {
            if (option != null)
            {
                changes.ToRemove.Add(option);
            }
        }

        private void CheckUniquenessName(string name)
        {
            if (categories.SelectMany(c => c.Variations).Any(v => v.Name == name))
            {
                throw new NonUniqueNameException(typeof(VariationOption));
            }
        }
    }
}