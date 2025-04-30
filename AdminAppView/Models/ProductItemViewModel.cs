namespace AdminAppView.Models
{
	public class ProductItemViewModel(int id, decimal price, IEnumerable<VariationOptionViewModal> options)
	{
		public int Id { get; set; } = id;
		public decimal Price { get; set; } = price;
		public IEnumerable<VariationOptionViewModal> Options { get; set; } = options;
	}
}
