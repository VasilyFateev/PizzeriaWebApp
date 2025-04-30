namespace AdminAppView.Models
{
	public class VariationViewModel(int id, string variationName, IEnumerable<VariationOptionViewModal> optionsList)
	{
		public int Id { get; set; } = id;
		public string Name { get; set; } = variationName;
		public IEnumerable<VariationOptionViewModal> OptionsList { get; set; } = optionsList;
	}
}
