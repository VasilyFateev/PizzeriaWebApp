using System.Text.RegularExpressions;

namespace ClientWebApp.Services
{
	public static partial class RegexCollection
	{

		[GeneratedRegex("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")]
		public static partial Regex EmailRegex();

		[GeneratedRegex("^(\\+7|7|8)?[\\s-]?\\(?[489][0-9]{2}\\)?[\\s-]?[0-9]{3}[\\s-]?[0-9]{2}[\\s-]?[0-9]{2}$")]
		public static partial Regex RussianPhoneRegex();

	}
}
