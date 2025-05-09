using AccountsModelClasses;
using AuthorizationService;

namespace ClientWebApp.Services
{
	public class AuthService
	{
		public async Task<IResult> HandleAuthRequest(HttpContext context)
		{
			var message = "Incorrect arguments";
			try
			{
				var authorizationData = await context.Request.ReadFromJsonAsync<ProvidedAuthorizationData>();
				if (authorizationData != null)
				{
					UserDataContext db = new();
					var controller = new AuthorizationController(db);

					var login = authorizationData.Login;
					User? user = login switch
					{
						string when RegexCollection.EmailRegex().IsMatch(login) => await controller.AuthorizeViaEmail(authorizationData.Login, authorizationData.Password),
						string when RegexCollection.RussianPhoneRegex().IsMatch(login) => await controller.AuthorizeViaPhonenNumber(authorizationData.Login, authorizationData.Password),
						_ => null
					};
					if (user is not null)
					{
						message = $"Hello, {user.Name}!";
					}
					else
					{
						message = "Invalid Login";
					}
				}
			}
			catch { }

			return Results.Json(new { text = message });
		}
		record ProvidedAuthorizationData(string Login, string Password);
	}
}
