namespace AdminAppView.Models
{
	public class ProductEditResponceVM
	{
		public class ConfigurationVM
		{
			public int ItemId { get; set; } = -1;
			public int OptionId { get; set; } = -1;
		}
		public class ProductItemVM
		{
			public int Id { get; set; }
			public int Price { get; set; }
			public int CookingTime { get; set; }
			public ConfigurationVM[] Configurations { get; set; } = [];
		}

		public int Id { get; set; } = -1;
		public string Name { get; set; } = "";
		public string Description { get; set; } = "Empty";
		public ProductItemVM[] ProductItems { get; set; } = [];
	}
}
