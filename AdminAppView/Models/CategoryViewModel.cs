namespace AdminAppView.Models
{
	public class CategoryViewModel(int categoryID, string categoryName, IEnumerable<ProductViewModel> productsList)
	{
		public int Id { get; set; } = categoryID;
		public string CategoryName { get; set; } = categoryName;
		public IEnumerable<ProductViewModel> ProductsList { get; set; } = productsList;
	}
}
