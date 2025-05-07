namespace AdminAppView.Models
{
	public class ProductEditRequestVM
	{
		public class VariationVM
		{
			public int Id { get; set; } = -1;
			public string Name { get; set; } = "";
			public VariationOptionVM[] VariationOptions { get; set; } = [];
		}
		public class VariationOptionVM
		{
			public int Id { get; set; } = -1;
			public int VariationID { get; set; } = -1;
			public string Name { get; set; } = "";
		}
		public class ConfigurationVM
		{
			public int ItemId { get; set; } = -1;
			public int OptionId { get; set; } = -1;
		}
		public class ProductItemVM
		{
			public int Id { get; set; }
			public decimal Price { get; set; }
			public int CookingTime { get; set; }
			public ConfigurationVM[] Configurations { get; set; } = [];
		}

		public int Id { get; set; } = -1;
		public string Name { get; set; } = "";
		public string? Description { get; set; } = "Empty";
		public VariationVM[] CategoryVariations { get; set; } = [];
		public ProductItemVM[] ProductItems { get; set; } = [];
	}
}
