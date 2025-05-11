using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductModelClasses
{
    [Table("variation")]
    public class Variation : Interfaces.IHasPrimaryKey, Interfaces.IHasUniqueName
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("product_category_id")]
        public int CategoryId { get; set; } = default!;

        [Required]
        [Column("name")]
        public string Name { get; set; } = null!;
        public ProductCategory Category { get; set; } = null!;
        public ICollection<VariationOption> VariationOptions { get; set; } = null!;
		public int GetPrimaryKey() => Id;
		public string GetUniqueName() => Name;
	}
}
