using AssortmentDatabaseAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AssortementDataContext>(options => options.UseNpgsql(DatabaseConnectionString.AssortmentDatabaseConnectionString));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization(); 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Assortment}/{action=AssortmentList}/");

app.MapDefaultControllerRoute();
app.Run();
