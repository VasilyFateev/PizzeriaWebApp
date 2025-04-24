using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelClasses
{
    [Table("variation")]
    public class Variation
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("FK_variation_product_category_id")]
        [Column("product_category_id")]
        public int CategoryId { get; set; } = default!;

        [Required]
        [Column("name")]
        public string Name { get; set; } = null!;
        public ProductCategory Category { get; set; } = null!;
        public ICollection<VariationOption> VariationOptions { get; set; } = null!;
    }

}
