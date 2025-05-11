namespace ClientWebApp.Models
{
    public class AssortmentListRequestVM
    {
        public class CategoryVM
        {
            public int ID { get; set; } = default!;
            public string Name { get; set; } = default!;
            public ProductVM[] Products { get; set; } = [];
        }

        public class ProductVM
        {
            public int ID { get; set; } = default!;
            public string Name { get; set; }= default!;
            public string Description { get; set; } = default!;
            public decimal MinPrice { get; set; } = default!;
        }

        public CategoryVM[] Categories { get; set; } = [];
    }
}
