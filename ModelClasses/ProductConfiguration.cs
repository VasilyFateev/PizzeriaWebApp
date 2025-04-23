using System.ComponentModel.DataAnnotations.Schema;

namespace ModelClasses
{
    [Table("product_configuration")]
    public class ProductConfiguration
    {
        [ForeignKey("FK_product_configuration_product_item_id")]
        [Column("product_item_id")]
        public ProductItem ProductItemId { get; set; } = null!;

        [ForeignKey("FK_product_configuration_variation_optionn_id")]
        [Column("variation_option_id")]
        public VariationOption VariationOptionId { get; set; } = null!;
    }

}
