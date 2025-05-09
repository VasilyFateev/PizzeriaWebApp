using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsModelClasses
{
	[Table("user_adress")]
	public class UserAdress
	{
		[Column("user_id")]
		public long UserId { get; set; } = default!;
		[Column("adress_id")]
		public long AdresssId { get; set; } = default!;
		[Column("id_default")]
		public bool IsDefault { get; set; }

		public User User { get; set; } = null!;
		public Adress Adress { get; set; } = null!;
	}

}
