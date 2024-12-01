using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MonopHelper.Areas.Admin.Services;
using MonopHelper.Data;
using MonopHelper.Authentication;
using MonopHelper.Authentication.UserClaims;
using MonopHelper.Helpers;
using MonopHelper.Helpers.GameDefaults;
using MonopHelper.Middleware;
using MonopHelper.Models.GameDb.Cards;
using MonopHelper.Services;
using MonopHelper.Services.Cards;
using MonopHelper.Services.InGame;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

builder.Services.AddSingleton<MonopHelper.Services.Version>();
builder.Services.AddTransient<AdminService>();

builder.Services.AddTransient<GameService>();
builder.Services.AddTransient<PlayerService>();
builder.Services.AddTransient<PropertyService>();
builder.Services.AddTransient<LoanService>();

builder.Services.AddTransient<ShuffleList<Card>>();

builder.Services.AddTransient<CardService>();
builder.Services.AddTransient<CardGameService>();

builder.Services.AddScoped<GameDbSet<Card>>();
builder.Services.AddScoped<GameDbSet<CardType>>();
builder.Services.AddScoped<GameDbSet<CardDeck>>();
builder.Services.AddScoped<GameDbSet<CardGame>>();
builder.Services.AddScoped<GameDbSet<CardToGame>>();
builder.Services.AddScoped<GameDbSet<TypeToGame>>();

builder.Services.AddScoped<UploadCardsService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

var gameDb = builder.Configuration.GetConnectionString("Game") ??
                       throw new InvalidOperationException("Connection string 'Game' not found.");
builder.Services.AddDbContext<GameDbContext>(options =>
    options.UseSqlite(gameDb));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false; //TODO: Implement email confirmation
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequiredLength = 8;
        options.User.RequireUniqueEmail = true;
        options.Lockout.MaxFailedAccessAttempts = 5;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<UserInfo>(sp => new UserInfo(sp));
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, GameUserClaimsPrincipalFactory>();

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

// var cookiePolicyOptions = new CookiePolicyOptions
// {
//     Secure = CookieSecurePolicy.Always,
//     MinimumSameSitePolicy = SameSiteMode.None
// };
// app.UseCookiePolicy(cookiePolicyOptions);

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//app.UseSession();
app.UseUserInfo();

app.MapRazorPages();
app.MapControllerRoute(name: "default", pattern: "{controller=Game}/{action=Index}");
app.MapControllerRoute(name: "admin", pattern: "{controller=Admin}/{action=Index}");
app.MapControllerRoute(name: "card", pattern: "{controller=Card}/{action=Index}");

Defaults(app).Wait();

app.Run();

async Task Defaults(IApplicationBuilder a)
{
    using var scope = app.Services.CreateScope();
    var sp = scope.ServiceProvider;
    var context = sp.GetRequiredService<ApplicationDbContext>();
    var gameContext = sp.GetRequiredService<GameDbContext>();
    var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();

    //Migrate Database:
    await context.Database.MigrateAsync();
    await gameContext.Database.MigrateAsync();

    
    async Task ConfirmRoleSetup(string role)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
    
    //Create tenant:
    var noTenant = await context.Tenants.FirstOrDefaultAsync(t => t.TenantName == "NO_TENANT");
    if (noTenant == null)
    {
        noTenant = new Tenant
        {
            TenantName = "NO_TENANT",
            DateCreated = DateTime.Now,
            IsDeleted = false
        };
        await context.Tenants.AddAsync(noTenant);
        await context.SaveChangesAsync();
    }

    //Create role:
    await ConfirmRoleSetup("Admin");
    
    //Create admin user:
    var adminUser = await userManager.FindByNameAsync("serveradmin");
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            Email = "jcraik23@gmail.com",
            UserName = "serveradmin",
            EmailConfirmed = true,
            TwoFactorEnabled = false,
            DisplayName = "Admin",
            TenantId = 1
        };
        await userManager.CreateAsync(adminUser);
        await userManager.AddToRoleAsync(adminUser, "Admin");
        var p = "TempPassword23@Helperv1.2";
        await userManager.AddPasswordAsync(adminUser, p);
        Console.WriteLine("=============================");
        Console.WriteLine("-----------------------------");
        Console.WriteLine("-----------------------------");
        Console.WriteLine("-----------------------------");
        Console.WriteLine(p);
        Console.WriteLine("-----------------------------");
        Console.WriteLine("-----------------------------");
        Console.WriteLine("-----------------------------");
        Console.WriteLine("=============================");
    }
    
    
    /*
     * Game Defaults:
     */
    var cardDefaults = new CardDefaults(gameContext);
    await cardDefaults.EnsureDefaults();
}