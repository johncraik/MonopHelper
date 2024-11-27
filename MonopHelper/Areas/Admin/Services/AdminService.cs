using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MonopHelper.Authentication;
using MonopHelper.Data;

namespace MonopHelper.Areas.Admin.Services;

public class AdminService
{
    private readonly ApplicationDbContext _appContext;
    private readonly GameDbContext _gameContext;

    public AdminService(ApplicationDbContext appContext, GameDbContext gameContext)
    {
        _appContext = appContext;
        _gameContext = gameContext;
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
        return await _appContext.Tenants.OrderBy(t => t.Id == 1).ThenBy(t => t.Id).ToListAsync();
    }

    public async Task<ApplicationUser?> GetUser(string id)
    {
        return await _appContext.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> ChangeUserTenant(string userId, int tenantId)
    {
        var user = await GetUser(userId);
        if (user == null) return false;

        user.TenantId = tenantId;
        _appContext.Users.Update(user);
        await _appContext.SaveChangesAsync();

        return true;
    }
}
            