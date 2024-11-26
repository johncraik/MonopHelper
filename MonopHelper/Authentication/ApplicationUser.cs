using Microsoft.AspNetCore.Identity;

namespace MonopHelper.Authentication;

public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }
    public int TenantId { get; set; }
}