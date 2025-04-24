using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelClasses
{
    [Keyless]
    [Table("product_configuration")]
    public class ProductConfiguration
    {
        [ForeignKey("FK_product_configuration_product_item_id")]
        [Column("product_item_id")]
        public int ProductItemId { get; set; } = default!;

        [ForeignKey("FK_product_configuration_variation_optionn_id")]
        [Column("variation_option_id")]
        public int VariationOptionId { get; set; } = default!;
    }

}
