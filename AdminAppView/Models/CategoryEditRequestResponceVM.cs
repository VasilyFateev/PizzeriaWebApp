namespace AdminAppView.Models
{
	public class CategoryEditRequestResponceVM
	{
		public class ProductVM
		{
			public int Id { get; set; } = -1!;
			public string Name { get; set; } = ""!;
		}
		public class VariationVM
		{
			public int Id { get; set; } = -1!;
			public string Name { get; set; } = ""!;
			public VariationOptionVM[] Options { get; set; } = []!;
		}
		public class VariationOptionVM
		{
			public int Id { get; set; } = -1!;
			public string Name { get; set; } = ""!;
		}
		public int Id { get; set; } = -1!;
		public string Name { get; set; } = ""!;
		public ProductVM[] Products { get; set; } = []!;
		public VariationVM[] Variations { get; set; } = []!;
	}
}
