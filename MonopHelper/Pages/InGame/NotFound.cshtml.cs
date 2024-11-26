using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MonopHelper.Pages.InGame;

[Authorize]
public class NotFound : PageModel
{
    public void OnGet()
    {
        
    }
}