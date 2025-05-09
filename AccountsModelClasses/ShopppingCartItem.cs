using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsModelClasses
{
	[Table("shopping_cart_item")]
	public class ShopppingCartItem
	{
		[Column("id")]
		public long Id { get; set; } = default!;
		[Column("cart_id")]
		public long CartId { get; set; } = default!;
		[Column("product_item_id")]
		public long ProductItemId { get; set; } = default!;
		[Column("count")]
		public long Count { get; set; } = default;
		public ShopppingCart ShopppingCart { get; set; } = null!;
	}

}
