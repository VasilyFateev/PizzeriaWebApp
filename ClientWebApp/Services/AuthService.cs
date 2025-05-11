using AccountsModelClasses;
using AuthorizationService;
using ClientWebApp.Services.Utility;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ClientWebApp.Services
{
	public class AuthService
	{
		public async Task<IResult> HandleAuthRequest(HttpContext context)
		{
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
						var claims = new List<Claim> { new(ClaimTypes.Name, login) };
						var jwt = new JwtSecurityToken(
								issuer: AuthOptions.ISSUER,
								audience: AuthOptions.AUDIENCE,
								claims: claims,
								expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
								signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
						var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                        context.Response.Cookies.Append("access_token", encodedJwt, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTime.UtcNow.AddHours(1)
                        });

                        await context.Response.WriteAsJsonAsync(new { access_token = encodedJwt });
                        return Results.Ok();
					}
					else
					{
						return Results.Unauthorized();
					}
				}
			}
			catch { }

			return Results.BadRequest();
		}
		record ProvidedAuthorizationData(string Login, string Password);
	}
}
