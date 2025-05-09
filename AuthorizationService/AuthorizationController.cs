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

		public async Task<bool> Registrate(User user, string providedPassword)
		{
			var sameUser = await context.Users.FirstOrDefaultAsync(u => (u.Email == user.Email && u.Email != string.Empty)
				|| (u.PhoneNumber == user.PhoneNumber && u.PhoneNumber != string.Empty));
			if (sameUser != null)
			{
				return false;
			}
			PasswordHasher<User> passwordHasher = new();
			var hashedPassword = passwordHasher.HashPassword(user, providedPassword);
			user.HashedPassword = hashedPassword;
			await context.Users.AddAsync(user);
			await context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> SetNewPassword(int id, string providedPassword)
		{
			var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
			if (user == null)
			{
				return false;
			}

			PasswordHasher<User> passwordHasher = new();
			var result = passwordHasher.VerifyHashedPassword(user, user.HashedPassword, providedPassword);
			if (result == PasswordVerificationResult.Success)
			{
				return false;
			}

			var hashedPassword = passwordHasher.HashPassword(user, providedPassword);
			user.HashedPassword = hashedPassword;
			await context.SaveChangesAsync();
			return true;
		}
	}
}
