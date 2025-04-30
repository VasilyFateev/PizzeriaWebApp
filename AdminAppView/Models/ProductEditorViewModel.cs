using ModelClasses;

namespace AdminAppView.Models
{
	public class ProductEditorViewModel(Product product)
	{
		public ProductViewModel Product { get; set; } = new ProductViewModel(
			product.Id,
			product.Name,
			product.Category.Variations.Select(variation => new VariationViewModel(
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
				product.Category.Variations
					.SelectMany(variation => variation.VariationOptions)
						.Where(option => option.Configurations
						.Any(configuration => configuration.ProductItemId == item.Id))
					.Select(option => new VariationOptionViewModal(option.Id, option.Name, true)
				))
			));
		public Variation[] Variations = [.. product.Category.Variations];
	}
}
