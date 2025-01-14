using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonopolyCL.Models.Cards;
using MonopolyCL.Services.Cards;

namespace MonopHelper.Pages.Cards;

[Authorize]
public class Index : PageModel
{
    private readonly CardService _cardService;
    private readonly CardGameService _cardGameService;

    public Index(CardService cardService, CardGameService cardGameService)
    {
        _cardService = cardService;
        _cardGameService = cardGameService;
    }

    [DisplayName("Card Deck")]
    public List<SelectListItem> CardDecks { get; set; }
    public List<CardGame> CardGames { get; set; }
    
    [BindProperty]
    public int SelectedDeck { get; set; }
    
    public async Task SetupPage()
    {
        CardDecks = (await _cardService.GetCardDecks()).Select(d => new SelectListItem
        {
            Text = d.Name,
            Value = d.Id.ToString()
        }).ToList();
        CardGames = await _cardGameService.GetCardGames();
    }
    
    public async Task<IActionResult> OnGet()
    {
        await SetupPage();
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        await SetupPage();
        var deck = await _cardService.FindCardDeck(SelectedDeck, true);
        if (deck != null)
        {
            var game = await _cardGameService.CreateGame(deck.Id);
            if (game != null) return RedirectToPage($"Play/{nameof(Play.CardGame)}", new { id = game.Game.Id });
        }

        ModelState["SelectedDeck"]?.Errors.Add("This card deck cannot be found. Please choose a different deck.");
        return Page();
        
    }
}