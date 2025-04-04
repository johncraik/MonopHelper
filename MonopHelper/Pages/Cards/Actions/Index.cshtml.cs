using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonopolyCL.Services.Cards;


namespace MonopHelper.Pages.Cards.Actions;

public class Index : PageModel
{
    private readonly CardActionService _cardActionService;

    public Index(CardActionService cardActionService)
    {
        _cardActionService = cardActionService;
    }
    
    public IActionResult OnGet(int id) => Page();

    public async Task<IActionResult> OnPost(int id)
    {
        var actions = await _cardActionService.GetCardActions(id);
        return Page();
    }
}