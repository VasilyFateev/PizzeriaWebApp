using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsModelClasses
{
	[Table("shopping_cart")]
	public class ShopppingCart
	{
		[Key]
		[Column("id")]
		public long Id { get; set; } = default!;
		[Column("user_id")]
		public long UserId { get; set; } = default!;

		public ICollection<ShopppingCartItem> Items { get; set; } = [];
		public User User { get; set; } = null!;
	}

}
