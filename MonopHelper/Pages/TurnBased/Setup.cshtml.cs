using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonopHelper.Authentication;
using MonopHelper.Pages.Cards;
using MonopolyCL.Data;
using MonopolyCL.Models.Game;
using MonopolyCL.Services.Game;

namespace MonopHelper.Pages.TurnBased;

public class Setup : PageModel
{
    private readonly MonopolyGameService _gameService;
    private readonly UserInfo _userInfo;

    public Setup(MonopolyGameService gameService, UserInfo userInfo)
    {
        _gameService = gameService;
        _userInfo = userInfo;
    }
    
    public MonopolyGame Game { get; set; }
    
    public async Task<IActionResult> OnGet(int id)
    {
        var game = await _gameService.FetchGame(id);
        if (game == null) return RedirectToPage(nameof(Index));

        var turnOrder = await _gameService.GetGameTurnOrder(game.Game.Id);
        if (turnOrder == null)
        {
            turnOrder = new TurnOrder
            {
                GameId = game.Game.Id,
                TenantId = _userInfo.TenantId,
                IsSetup = false
            };
            await _gameService.CreateGameTurnOrder(turnOrder);
        }
        if (turnOrder.IsSetup) return RedirectToPage(nameof(Play), new {id = game.Game.Id});
        
        Game = game;
        return Page();
    }
}