using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MonopHelper.Data;
using MonopHelper.Models;
using MonopHelper.Services;
using MonopHelper.Services.InGame;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<GameService>();
builder.Services.AddTransient<PlayerService>();
builder.Services.AddTransient<PropertyService>();
builder.Services.AddTransient<LoanService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllers();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(name: "default", pattern: "{controller=Game}/{action=Index}");

app.Run();