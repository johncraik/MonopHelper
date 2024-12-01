using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonopHelper.Authentication;

namespace MonopHelper.Pages;

[Authorize(Roles = $"{GameRoles.AccessVh}, {GameRoles.Admin}")]
public class Version : PageModel
{
    public void OnGet()
    {
        
    }
}