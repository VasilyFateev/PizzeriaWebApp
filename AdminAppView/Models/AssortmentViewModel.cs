using ModelClasses;

namespace AdminAppView.Models
{
	public class AssortmentListViewModel(List<ProductCategory> categories)
	{
		public CategoryViewModel[] CategoryListElements { get; set; } = categories.Select(category => new CategoryViewModel(
				category.Id,
				category.Name,
				category.Products.Select(product => new ProductViewModel(
					product.Id,
					product.Name,
					category.Variations.Select(variation => new VariationViewModel(
						variation.Id,
						variation.Name,
						variation.VariationOptions.Select(option => new VariationOptionViewModal(
							option.Id,
							option.Name,
							product.ProductItems
								.SelectMany(productItem => productItem.Configurations)
								.Any(configuration => configuration.VariationOptionId == option.Id)
							))
						)),
					product.ProductItems.Select(item => new ProductItemViewModel(
						item.Id,
						item.Price,
						category.Variations
							.SelectMany(variation => variation.VariationOptions)
								.Where(option => option.Configurations
								.Any(configuration => configuration.ProductItemId == item.Id))
							.Select(option => new VariationOptionViewModal(option.Id, option.Name, true)

						)
					))
				))
			)).ToArray();
	}
}
