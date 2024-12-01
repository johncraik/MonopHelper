using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MonopHelper.Pages.Cards.Play;

[Authorize]
public class NotFound : PageModel
{
    public void OnGet()
    {
        
    }
}