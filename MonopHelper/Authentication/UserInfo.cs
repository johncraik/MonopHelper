using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace MonopHelper.Authentication;

public class UserInfo
{
    private readonly IServiceProvider _serviceProvider;

    public UserInfo(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
    }
    
    public string UserId { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public int TenantId { get; set; }
    public bool IsSetup { get; set; }
    public string DisplayName { get; set; }

    private ApplicationUser? _user;
    public ClaimsPrincipal ClaimsPrincipal { get; set; }

    public async Task<ApplicationUser?> GetApplicationUser()
    {
        return _user ??= await _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>()
            .GetUserAsync(ClaimsPrincipal);
    }
}