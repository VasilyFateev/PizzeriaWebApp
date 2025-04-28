using ModelClasses;


namespace AdminAppView.Models
{
	public class AssortmentListViewModel(List<ProductCategory> categories)
    {
        public CategoryListElement[] CategoryListElements { get; set; } = categories.Select(category => new CategoryListElement(
                category.Id,
                category.Name,
                category.Products.Select(product => new ProductListElement(
                    product.Id,
                    product.Name,
                    category.Variations.Select(variation => new VariationOptionList(
                        variation.Name,
                        variation.VariationOptions.Select(option => new VariationOptionListElement(
                            option.Name,
                            product.ProductItems
                                .SelectMany(productItem => productItem.Configurations)
                                .Any(configuration => configuration.VariationOptionId == option.Id)
                            ))
                        ))
                    ))
                ))
                .ToArray();

        public class CategoryListElement(int categoryID, string categoryName, IEnumerable<ProductListElement> productsList)
		{
			public int CategoryID { get; set; } = categoryID;
			public string CategoryName { get; set; } = categoryName;
			public IEnumerable<ProductListElement> ProductsList { get; set; } = productsList;
		}

		public class ProductListElement(int productID, string productName, IEnumerable<VariationOptionList> variationList)
		{
			public int ProductID { get; set; } = productID;
			public string ProductName { get; set; } = productName;
			public IEnumerable<VariationOptionList> VariationList { get; set; } = variationList;
		}

		public class VariationOptionList(string variationName, IEnumerable<VariationOptionListElement> optionsList)
		{
			public string VariationName { get; set; } = variationName;
			public IEnumerable<VariationOptionListElement> OptionsList { get; set; } = optionsList;
		}

		public class VariationOptionListElement(string variationOptionName, bool status)
		{
			public string VariationOptionName { get; set; } = variationOptionName;
			public bool Status { get; } = status;
		}
	}
}
