using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MonopHelper.Pages.Cards;

[Authorize]
public class Index : PageModel
{
    public void OnGet()
    {
        
    }
}