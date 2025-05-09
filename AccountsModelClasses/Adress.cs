using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsModelClasses
{
	[Table("adress")]
	public class Adress
	{
		[Column("id")]
		public long Id { get; set; } = default!;
		[Column("region")]
		public string Region { get; set; } = default!;
		[Column("settlement")]
		public string Settlement { get; set; } = default!;
		[Column("street_name")]
		public string StreetName { get; set; } = default!;
		[Column("building_number")]
		public int BuildingNumber { get; set; } = default!;
		[Column("liter")]
		public string? Liter { get; set; }
		[Column("apartment_number")]
		public int? ApartmentNumber { get; set; }
		public ICollection<UserAdress> UserAdresses { get; set; } = [];
	}

}
