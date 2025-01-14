using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonopolyCL.Models.Game;
using MonopolyCL.Services.Game;

namespace MonopHelper.Areas.Admin.Pages;

public class Test : PageModel
{
    private readonly MonopolyGameService _monopolyGameService;

    public Test(MonopolyGameService monopolyGameService)
    {
        _monopolyGameService = monopolyGameService;
    }
    
    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPost()
    {
        var players = new List<string>();
        players.AddRange(["John", "Dad"]);

        //var game = await _monopolyGameService.CreateGame(1, players, (GAME_RULES)0);

        var fetchGame = await _monopolyGameService.FetchGame(5);
        
        return Page();
    }
}