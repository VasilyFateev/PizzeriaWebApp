using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsModelClasses
{
	[Table("user")]
	public class User
	{
		[Column("id")]
		public long Id { get; set; } = default!;
		[Column("name")]
		public string Name { get; set; } = default!;
		[Column("phone_number")]
		public string PhoneNumber { get; set; } = default!;
		[Column("email")]
		public string? Email { get; set; }
		[Column("cached_passsword")]
		public string CachedPasssword { get; set; } = default!;

		public ICollection<UserAdress> UserAdresses { get; set; } = null!;
		public ICollection<UserPaymentMethod> UserPaymentMethods { get; set; } = null!;
		public ICollection<ShopppingCart> ShopppingCarts { get; set; } = null!;
	}

}
