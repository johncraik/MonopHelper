using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonopolyCL.Models.Game;
using MonopolyCL.Services.Game;

namespace MonopHelper.Pages.TurnBased;

[Authorize]
public class Win : PageModel
{
    private readonly MonopolyGameService _gameService;

    public Win(MonopolyGameService gameService)
    {
        _gameService = gameService;
    }
    
    public MonopolyGame Game { get; set; }

    public async Task<IActionResult?> SetupPage(int id)
    {
        var game = await _gameService.FetchGame(id);
        if (game == null) return RedirectToPage(nameof(Index));

        if (game.Players.Count > 1) return RedirectToPage(nameof(Play), new { id });

        Game = game;
        return null;
    }
    
    public async Task<IActionResult> OnGet(int id)
    {
        var redirect = await SetupPage(id);
        return redirect ?? Page();
    }

    public async Task<IActionResult> OnPost(int id)
    {
        var redirect = await SetupPage(id);
        if (redirect != null) return redirect;

        await _gameService.DeleteGame(id);
        return RedirectToPage(nameof(Index));
    }
}