﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsModelClasses
{
	[Table("user")]
	public class User
	{
		[Key]
		[Column("id")]
		public long Id { get; set; } = default!;
		[Column("name")]
		public string Name { get; set; } = default!;
		[Column("phone_number")]
		public string PhoneNumber { get; set; } = default!;
		[Column("email")]
		public string? Email { get; set; }
		[Column("hashed_passsword")]
		public string HashedPassword { get; set; } = default!;

		public ICollection<UserAdress> UserAdresses { get; set; } = null!;
		public ICollection<UserPaymentMethod> UserPaymentMethods { get; set; } = null!;
		public ICollection<ShopppingCart> ShopppingCarts { get; set; } = null!;
	}
}
