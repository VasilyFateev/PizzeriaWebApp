namespace AdminAppView.Models
{
	public class ProductViewModel(int productID, string productName, IEnumerable<VariationViewModel> variationList, IEnumerable<ProductItemViewModel> itemsList)
	{
		public int Id { get; set; } = productID;
		public string Name { get; set; } = productName;
		public IEnumerable<VariationViewModel> VariationList { get; set; } = variationList;
		public IEnumerable<ProductItemViewModel> ItemsList { get; set; } = itemsList;
	}
}
