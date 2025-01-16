using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonopolyCL.Models.Game;
using MonopolyCL.Services.Game;

namespace MonopHelper.Pages.TurnBased;

public class Play : PageModel
{
    private readonly MonopolyGameService _gameService;

    public Play(MonopolyGameService gameService)
    {
        _gameService = gameService;
    }
    
    public MonopolyGame Game { get; set; }
    
    public async Task<IActionResult> OnGet(int id)
    {
        var game = await _gameService.FetchGame(id);
        if (game == null) return RedirectToPage(nameof(Index));
        
        var turnOrder = await _gameService.GetGameTurnOrder(game.Game.Id);
        if(turnOrder == null) return RedirectToPage(nameof(Index));
        if (!turnOrder.IsSetup) return RedirectToPage(nameof(Setup), new {id = game.Game.Id});

        Game = game;
        return Page();
    }
}