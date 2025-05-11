using AuthorizationService;

namespace ConsoleView.Commands.Authorization
{
	internal class СсPhoneRegisterUser : IConsoleCommand
	{
		public string ConsoleGroupName => "Authorization";
		public string Name => "regPhone";
		public string Description => "[INFO] Syntax: reg [PhoneNumber:string] [Name:string] [Password:string]";

		public async Task<string> Execute(params string[] args)
		{
			UserDataContext db = new();
			if (db.Database.CanConnect())
			{
				using (db = new())
				{
					var controller = new RegistrationController(db);
					if (args.Length != 3)
					{
						return $"[ERROR] Incorrect argument count.";
					}

					var result = await controller.Registrate(new AccountsModelClasses.User()
					{
						Email = "",
						PhoneNumber = args[0],
						Name = args[1],
					},
					args[2]);

					if (result)
					{
						return $"[INFO] Account is registered.";
					}
					else
					{
						return $"[ERROR] There is an account with this phone number.";
					}
				}
			}
			else
			{
				return $"[ERROR] Database connection error.";
			}
		}
	}
}
