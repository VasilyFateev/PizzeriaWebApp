using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelClasses
{
    [Table("variation_option")]
    public class VariationOption
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("FK_variation_option_variation_id")]
        [Column("variation_id")]
        public Variation VariationId { get; set; } = null!;

        [Required]
        [Column("name")]
        public string Name { get; set; } = null!;
        public ProductCategory Variation { get; set; } = null!;
    }

}
