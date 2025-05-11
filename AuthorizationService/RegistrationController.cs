using AccountsModelClasses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationService
{
    public class RegistrationController(UserDataContext context)
	{
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
    }
}
