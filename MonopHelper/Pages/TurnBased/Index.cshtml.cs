using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MonopHelper.Pages.TurnBased;

public class Index : PageModel
{
    public class TurnBasedView
    {
        [DisplayName("Board")]
        public string BoardName { get; set; }
        public string Players { get; set; }
        public DateTime LastPlayed { get; set; }
    }
    
    public async Task<IActionResult> OnGet()
    {
        return Page();
    }
}