using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MonopHelper.Pages;

[Authorize]
public class Version : PageModel
{
    public void OnGet()
    {
        
    }
}