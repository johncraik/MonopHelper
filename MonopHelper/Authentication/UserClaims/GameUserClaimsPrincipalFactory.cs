using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace MonopHelper.Authentication.UserClaims;

public class GameUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
{
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var defaultClaims = await base.GenerateClaimsAsync(user);
        
        defaultClaims.AddClaim((new Claim(GameClaims.DisplayNameClaim, user.DisplayName ?? "")));
        defaultClaims.AddClaim((new Claim(GameClaims.TenantId, user.TenantId.ToString())));

        return defaultClaims;
    }

    public GameUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options)
        : base (userManager, roleManager, options)
    {
        
    }
}