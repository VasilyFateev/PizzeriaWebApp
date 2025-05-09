using AuthorizationService;

namespace ConsoleView.Commands.Authorization
{
	internal class СсEmailRegisterUser : IConsoleCommand
	{
		public string ConsoleGroupName => "Authorization";
		public string Name => "regEmail";
		public string Description => "[INFO] Syntax: reg [E-mail:string] [Name:string] [Password:string]";

		public async Task<string> Execute(params string[] args)
		{
			UserDataContext db = new();
			if (db.Database.CanConnect())
			{
				using (db = new())
				{
					var controller = new AuthorizationController(db);
					if (args.Length != 3)
					{
						return $"[ERROR] Incorrect argument count.";
					}

					var result = await controller.Registrate(new AccountsModelClasses.User()
					{
						Email = args[0],
						PhoneNumber = "",
						Name = args[1],
					},
					args[2]);

					if (result)
					{
						return $"[INFO] Account is registered.";
					}
					else
					{
						return $"[ERROR] There is an account with this email address.";
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
