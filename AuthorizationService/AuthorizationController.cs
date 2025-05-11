using AccountsModelClasses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationService
{
    public class AuthorizationController(UserDataContext context)
	{
		public async Task<User?> AuthorizeViaEmail(string providedEmail, string providedPassword)
		{
			var user = await context.Users.FirstOrDefaultAsync(user => user.Email == providedEmail);
			if (user == null)
			{
				return null;
			}

			PasswordHasher<User> passwordHasher = new();
			return passwordHasher.VerifyHashedPassword(user, user.HashedPassword, providedPassword) switch
			{
				PasswordVerificationResult.Failed => null,
				_ => user,
			};
		}

		public async Task<User?> AuthorizeViaPhonenNumber(string providedPhoneNumber, string providedPassword)
		{
			var user = await context.Users.FirstOrDefaultAsync(user => user.PhoneNumber == providedPhoneNumber);
			if (user == null)
			{
				return null;
			}

			PasswordHasher<User> passwordHasher = new();
			return passwordHasher.VerifyHashedPassword(user, user.HashedPassword, providedPassword) switch
			{
				PasswordVerificationResult.Failed => null,
				_ => user,
			};
		}
	}
}
