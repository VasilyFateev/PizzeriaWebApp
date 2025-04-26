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
        public int VariationId { get; set; } = default!;

        [Required]
        [Column("name")]
        public string Name { get; set; } = null!;
        public Variation Variation { get; set; } = null!;

        public ICollection<ProductConfiguration> Configurations { get; set; } = [];
    }

}
