using Microsoft.AspNetCore.Identity;
using MonopolyCL.Models.Identity;

namespace MonopHelper.Authentication;

public class UserWithRoles
{
    public ApplicationUser User { get; set; }
    public List<IdentityRole> Roles { get; set; }
}