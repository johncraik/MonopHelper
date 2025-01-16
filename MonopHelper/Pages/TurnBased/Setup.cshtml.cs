using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonopHelper.Authentication;
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

    public class TurnOrderViewModel
    {
        public int PlayerId { get; set; }
        public int Order { get; set; }
        public int Dice1 { get; set; }
        public int Dice2 { get; set; }
    }
    
    public MonopolyGame Game { get; set; }
    public List<SelectListItem> PlayerOrders { get; set; }
    public List<TurnOrderViewModel> Input { get; set; }

    public async Task<IActionResult?> SetupPage(int id)
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
        
        for (var i = 1; i <= Game.Players.Count; i++)
        {
            PlayerOrders.Add(new SelectListItem
            {
                Text = i.ToString(),
                Value = i.ToString()
            });
        }

        return PlayerOrders.Count < 2 ? RedirectToPage(nameof(Index)) : null;
    }

    public async Task<IActionResult> OnGet(int id)
    { 
        var redirect = await SetupPage(id);
        if (redirect != null) return redirect;

        Input = new List<TurnOrderViewModel>();
        foreach (var p in Game.Players)
        {
            Input.Add(new TurnOrderViewModel
            {
                
            });
        }

        return Page();
    }
}