using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonopolyCL.Models.Game;
using MonopolyCL.Services.Game;

namespace MonopHelper.Pages.TurnBased;

public class Setup : PageModel
{
    private readonly MonopolyGameService _gameService;

    public Setup(MonopolyGameService gameService)
    {
        _gameService = gameService;
    }
    
    public MonopolyGame Game { get; set; }
    
    public async Task<IActionResult> OnGet(int id)
    {
        var game = await _gameService.FetchGame(id);
        if (game == null) return RedirectToPage(nameof(Index));

        Game = game;
        return Page();
    }
}