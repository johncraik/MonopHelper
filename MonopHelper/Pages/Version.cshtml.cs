using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonopHelper.Authentication;

namespace MonopHelper.Pages;

[Authorize(Roles = GameRoles.AccessVh)]
public class Version : PageModel
{
    public void OnGet()
    {
        
    }
}