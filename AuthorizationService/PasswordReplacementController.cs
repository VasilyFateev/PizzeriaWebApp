using AccountsModelClasses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationService
{
    public class PasswordReplacementController(UserDataContext context)
    {
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
