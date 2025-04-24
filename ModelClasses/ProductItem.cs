using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelClasses
{
    [Table("product_item")]
    public class ProductItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [ForeignKey("FK_product_item_product_id")]
        [Column("product_id")]
        public int ProductId { get; set; } = default!;
        [Column("cooking_time")]
        public int CookingTime { get; set; } = default!;
        [Column("price")]
        public decimal Price { get; set; } = default!;

        public Product Product { get; set; } = null!;
    }
}


