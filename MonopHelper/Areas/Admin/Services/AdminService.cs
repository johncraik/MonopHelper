using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopolyCL.Data;
using MonopolyCL.Models.Identity;

namespace MonopHelper.Areas.Admin.Services;

public class AdminService
{
    private readonly ApplicationDbContext _appContext;
    private readonly GameDbContext _gameContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _config;

    public AdminService(ApplicationDbContext appContext, GameDbContext gameContext,
        UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
        IConfiguration config)
    {
        _appContext = appContext;
        _gameContext = gameContext;
        _userManager = userManager;
        _roleManager = roleManager;
        _config = config;
    }

    public async Task<List<UserWithRoles>> GetAllUsers()
    {
        //Get user IDs and role IDs, grouped by user ID:
        var userRoles = from ur in _appContext.UserRoles
            group ur by ur.UserId
            into u
            select new
            {
                User = u.Key,
                Roles = u.Select(r => r.RoleId).ToList()
            };
        
        //Get users that do not have roles:
        var usersWithRoles = await _appContext.Users.Where(u => 
                !userRoles.Select(ur => ur.User).Contains(u.Id))
            .Select(u => new UserWithRoles
            {
                User = u,
                Roles = new List<IdentityRole>()
            }).ToListAsync();
        
        //Get all roles:
        var roles = await _appContext.Roles.ToListAsync();
        
        //Foreach user that has roles:
        foreach (var userRole in userRoles)
        {
            //Find user:
            var user = await _appContext.Users.FirstOrDefaultAsync(u => u.Id == userRole.User);
            if (user != null)
            {
                //Add user with roles to list:
                usersWithRoles.Add(new UserWithRoles
                {
                    User = user,
                    Roles = roles.Where(r => userRole.Roles.Contains(r.Id)).ToList()
                });
            }
        }
        
        //Return list of user and their roles:
        return usersWithRoles;
    }

    public async Task<List<Tenant>> GetTenants()
    {
        //Get all tenants, with NO_TENANT at bottom:
        return await _appContext.Tenants.OrderBy(t => t.IsDeleted)
            .ThenBy(t => t.Id == 1).ThenBy(t => t.Id).ToListAsync();
    }

    public async Task<UserWithRoles?> GetUser(string id)
    {
        var roles = await _appContext.Roles.ToListAsync();
        var user = await _appContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return null;

        var userRole = await _appContext.UserRoles.Where(ur => ur.UserId == user.Id)
            .ToListAsync();


        return new UserWithRoles
        {
            User = user,
            Roles = roles.Where(r => userRole.Select(ur => ur.RoleId).Contains(r.Id)).ToList()
        };
    }

    public async Task<bool> ChangeUserTenant(string userId, int tenantId)
    {
        var user = await GetUser(userId);
        if (user == null) return false;

        user.User.TenantId = tenantId;
        _appContext.Users.Update(user.User);
        await _appContext.SaveChangesAsync();

        return true;
    }

    public async Task<Tenant?> FindTenant(int id) => await _appContext.Tenants.FirstOrDefaultAsync(t => t.Id == id);
    public async Task<bool> ValidateNewTenant(string name) => !await _appContext.Tenants.AnyAsync(t => t.TenantName == name);

    public async Task CreateTenant(string name)
    {
        var tenant = new Tenant
        {
            TenantName = name,
            DateCreated = DateTime.Now,
            IsDeleted = false
        };
        await _appContext.Tenants.AddAsync(tenant);
        await _appContext.SaveChangesAsync();
    }

    public async Task SetTenantDeleted(Tenant tenant, bool delete = true)
    {
        tenant.IsDeleted = delete;
        _appContext.Tenants.Update(tenant);
        await _appContext.SaveChangesAsync();
    }

    public async Task<bool> RoleExists(string role) => await _roleManager.RoleExistsAsync(role);

    public async Task AddRoleToUser(string userId, string role)
    {
        var user = await GetUser(userId);
        if (user != null)
        {
            await _userManager.AddToRoleAsync(user.User, role);
        }
    }
    
    public async Task RemoveRoleFromUser(string userId, string role)
    {
        var user = await GetUser(userId);
        if (user != null)
        {
            await _userManager.RemoveFromRoleAsync(user.User, role);
            await SignOutUser(user.User);
        }
    }

    private async Task SignOutUser(ApplicationUser user)
    {
        await _userManager.UpdateSecurityStampAsync(user);
    }

    public async Task<string> ResetUserPassword(string userId)
    {
        var user = await _appContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return "ERROR! Cannot Reset Password!";

        var p = _config["reset_password"];
        if(p == null) return "ERROR! Cannot Reset Password!";

        user.PasswordHash = null;
        _appContext.Users.Update(user);
        await _appContext.SaveChangesAsync();
        
        var resetPassword = await _userManager.AddPasswordAsync(user, p);
        return !resetPassword.Succeeded ? "ERROR! Cannot Reset Password!" : p;
    }
}
            