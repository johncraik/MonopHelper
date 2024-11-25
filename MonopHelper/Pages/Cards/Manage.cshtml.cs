using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonopHelper.Models.GameDb.Cards;
using MonopHelper.Services.Cards;

namespace MonopHelper.Pages.Cards;

public class Manage : PageModel
{
    private readonly CardService _cardService;

    public Manage(CardService cardService)
    {
        _cardService = cardService;
    }
    
    public List<SelectListItem> CardDecks { get; set; }
    public List<Card> Cards { get; set; }
    
    public async Task<IActionResult> OnGet()
    {
        await PopulateData();
        
        return Page();
    }

    private async Task PopulateData()
    {
        var decks = await _cardService.GetCardDecks();
        CardDecks = decks.Select(d => new SelectListItem
        {
            Text = d.Name,
            Value = d.Id.ToString()
        }).ToList();

        Cards = await _cardService.GetCardsFromDeck(decks.FirstOrDefault()?.Id ?? 0);
    }
}