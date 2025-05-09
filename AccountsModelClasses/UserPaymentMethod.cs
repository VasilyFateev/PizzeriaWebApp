using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsModelClasses
{
	[Table("user_payment_method")]
	public class UserPaymentMethod
	{
		[Key]
		[Column("id")]
		public long Id { get; set; } = default!;
		[Column("user_id")]
		public long UserId { get; set; } = default!;
		[Column("payment_agregator_user_id")]
		public string PaymentAgregatorCardId { get; set; } = default!;
		[Column("bank_card_last_numbers")]
		public string BankCardLastNumbers { get; set; } = default!;
		[Column("id_default")]
		public bool IsDefault  { get; set; }

		public User User { get; set; } = null!;
	}

}
