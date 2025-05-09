using ClientWebApp.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<RegisterService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.MapPost("/api/auth", async (AuthService authService, HttpContext context) => await authService.HandleAuthRequest(context));
app.MapPost("/api/reg", async (RegisterService regService, HttpContext context) => await regService.HandleRegRequest(context));

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "authorization",
	pattern: "auth",
	defaults: new { controller = "Authorization", action = "Index" });

app.MapControllerRoute(
	name: "registration",
	pattern: "reg",
	defaults: new { controller = "Authorization", action = "Registration" });

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
