using KonjeticZavrsni.DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DriverManagerDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DriverManagerDbContext"),
            opt => opt.MigrationsAssembly("KonjeticZavrsni.DAL")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "home-privacy",
    pattern: "o-aplikaciji",
    defaults: new { controller = "Home", action = "Privacy" },
    constraints: new { selected = @"[a-zA-Z][a-zA-Z]{2}" });


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
