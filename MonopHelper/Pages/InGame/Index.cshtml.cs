using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonopHelper.Services;
using MonopHelper.Models;

namespace MonopHelper.Pages.InGame;

public class Index : PageModel
{
    private readonly GameService _gameManager;
    private readonly IConfiguration _config;

    public Index(GameService gameManager, IConfiguration config)
    {
        _gameManager = gameManager;
        _config = config;
    }
    
    public Game? CurrentGame { get; set; }
    
    public async Task<IActionResult> OnGet(int id)
    {
        var game = await _gameManager.FetchGame(id, _config["init_userId"] ?? "default");
        if (game == null) return RedirectToPage(nameof(NotFound));

        CurrentGame = game;
        
        return Page();
    }
}