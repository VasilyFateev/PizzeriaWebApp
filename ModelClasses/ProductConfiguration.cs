using System.ComponentModel.DataAnnotations.Schema;

namespace ModelClasses
{
    [Table("product_configuration")]
    public class ProductConfiguration
    {
        [Column("product_item_id")]
        public int ProductItemId { get; set; }

        [Column("variation_option_id")]
        public int VariationOptionId { get; set; }

        // Навигационные свойства
        public ProductItem ProductItem { get; set; } = null!;
        public VariationOption? VariationOption { get; set; }
    }
}
