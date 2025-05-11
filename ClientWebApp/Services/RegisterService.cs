using AccountsModelClasses;
using AuthorizationService;

namespace ClientWebApp.Services
{
	public class RegisterService
	{
		public async Task<IResult> HandleRegRequest(HttpContext context)
		{
			var message = "Invalid arguments";
			try
			{
				var authorizationData = await context.Request.ReadFromJsonAsync<ProvidedRegistationData>();
				if (authorizationData != null)
				{
					UserDataContext db = new();
					var controller = new RegistrationController(db);
					var login = authorizationData.Login;

					User? user = login switch
					{
						string when RegexCollection.EmailRegex().IsMatch(login) => new() { Email = login, Name = authorizationData.Name },
						string when RegexCollection.RussianPhoneRegex().IsMatch(login) => new() { PhoneNumber = login, Name = authorizationData.Name },
						_ => null
					};

					if (user is not null)
					{
						if (await controller.Registrate(user, authorizationData.Password))
						{
							message = $"Welcome to the club, {user.Name}!";
						}
						else
						{
							message = $"An account with this phone number or e-mail address has already been registered.";
						}
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
		record ProvidedRegistationData(string Login, string Name, string Password, string PasswordRepeat);
	}
}
