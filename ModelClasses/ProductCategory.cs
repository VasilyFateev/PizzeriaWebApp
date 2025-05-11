using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductModelClasses
{
    [Table("product_category")]
    public class ProductCategory : Interfaces.IHasPrimaryKey, Interfaces.IHasUniqueName
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; } = null!;

        public ICollection<Product> Products { get; set; } = null!;
        public ICollection<Variation> Variations { get; set; } = [];
		public int GetPrimaryKey() => Id;
		public string GetUniqueName() => Name;
	}

}
