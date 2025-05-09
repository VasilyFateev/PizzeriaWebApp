using AuthorizationService;

namespace ConsoleView.Commands.Authorization
{
	internal class СсPhoneAuthorizationUser : IConsoleCommand
	{
		public string ConsoleGroupName => "Authorization";
		public string Name => "authPhone";
		public string Description => "[INFO] Syntax: authPhone [PhoneNumber:string] [Password:string]";

		public async Task<string> Execute(params string[] args)
		{
			UserDataContext db = new();
			if (db.Database.CanConnect())
			{
				using (db = new())
				{
					var controller = new AuthorizationController(db);
					if (args.Length != 2)
					{
						return $"[ERROR] Incorrect argument count.";
					}
					var result = await controller.AuthorizeViaPhonenNumber(args[0], args[1]);

					if (result != null)
					{
						return $"[INFO] Hello ${result.Name}";
					}
					else
					{
						return $"[ERROR] Invalid login or password.";
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
