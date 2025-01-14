using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonopolyCL.Models.Cards.ViewModels;
using MonopolyCL.Services.Cards;

namespace MonopHelper.Pages.Cards.Play;

[Authorize]
public class CardGame : PageModel
{
    private readonly CardGameService _cardGameService;

    public CardGame(CardGameService cardGameService)
    {
        _cardGameService = cardGameService;
    }
    
    [BindProperty]
    public int TypeId { get; set; }
    [BindProperty]
    public int GameId { get; set; }
    
    public CardGameViewModel? CardGameView { get; set; }

    public async Task SetupPage(int id)
    {
        var game = await _cardGameService.FetchGame(id);
        CardGameView = game;
    }
    
    public async Task<IActionResult> OnGet(int id)
    {
        await SetupPage(id);
        if (CardGameView == null) return RedirectToPage(nameof(Play.NotFound));
        
        return Page();
    }

    public IActionResult OnPostCard()
    {
        return RedirectToPage(nameof(View), new { id = GameId, typeId = TypeId });
    }
}