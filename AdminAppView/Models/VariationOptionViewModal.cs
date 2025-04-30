namespace AdminAppView.Models
{
	public class VariationOptionViewModal(int id, string variationOptionName, bool status)
	{
		public int Id { get; set; } = id;
		public string Name { get; set; } = variationOptionName;
		public bool Status { get; } = status;
	}
}
