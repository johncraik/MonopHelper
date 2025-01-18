using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonopolyCL.Models.Game;
using MonopolyCL.Models.Players;
using MonopolyCL.Services.Game;
using MonopolyCL.Services.Players;

namespace MonopHelper.Pages.TurnBased;

public class Play : PageModel
{
    private readonly MonopolyGameService _gameService;
    private readonly PlayerService _playerService;

    public Play(MonopolyGameService gameService, PlayerService playerService)
    {
        _gameService = gameService;
        _playerService = playerService;
    }
    
    public MonopolyGame Game { get; set; }
    public TurnOrder Turn { get; set; }
    public IPlayer? CurrentPlayer { get; set; }
    
    public async Task<IActionResult> OnGet(int id)
    {
        var game = await _gameService.FetchGame(id);
        if (game == null) return RedirectToPage(nameof(Index));
        
        var turnOrder = await _gameService.GetGameTurnOrder(game.Game.Id);
        if(turnOrder == null) return RedirectToPage(nameof(Index));
        if (!turnOrder.IsSetup) return RedirectToPage(nameof(Setup), new {id = game.Game.Id});

        Turn = turnOrder;
        CurrentPlayer = game.Players.FirstOrDefault(p => p.Id == turnOrder.CurrentTurn);
        Game = game;
        return Page();
    }

    public async Task<IActionResult> OnPostEnd(int id)
    {
        var res = await _playerService.EndTurn(id);
        return res ? RedirectToPage(nameof(Play), new {id}) : RedirectToPage(nameof(Index));
    }
}