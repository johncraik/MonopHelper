using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace MonopHelper.Pages.Cards.Actions;

public class Index : PageModel
{
    public IActionResult OnGet() => Page();
}