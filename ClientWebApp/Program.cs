using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ClientWebApp.Services;
using ClientWebApp.Services.Utility;
using AssortmentDatabaseAccess;
using Microsoft.EntityFrameworkCore;
using AuthorizationService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AssortementDataContext>(options => options.UseNpgsql(AssortmentDatabaseAccess.DatabaseConnectionString.AssortmentDatabaseConnectionString));
builder.Services.AddDbContext<UserDataContext>(options => options.UseNpgsql(AuthorizationService.DatabaseConnectionString.UsersDatabaseConnectionString));
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<RegisterService>();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["access_token"];
                return Task.CompletedTask;
            }
        };
    });
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

var api = app.MapGroup("/api");
api.MapPost("/auth", async (AuthService authService, HttpContext context) => await authService.HandleAuthRequest(context));
api.MapPost("/reg", async (RegisterService regService, HttpContext context) => await regService.HandleRegRequest(context));

app.MapControllerRoute(
    name: "auth",
    pattern: "auth",
    defaults: new { controller = "Authorization", action = "Index" });

app.MapControllerRoute(
    name: "reg",
    pattern: "reg",
    defaults: new { controller = "Authorization", action = "Registration" });

app.MapControllerRoute(
    name: "test",
    pattern: "test",
    defaults: new { controller = "Assortment", action = "Index" });

app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
