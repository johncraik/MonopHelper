using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonopHelper.Authentication;
using MonopolyCL.Models.Game;
using MonopolyCL.Models.Validation;
using MonopolyCL.Services.Game;
using MonopolyCL.Services.Players;

namespace MonopHelper.Pages.TurnBased;

public class Setup : PageModel
{
    private readonly MonopolyGameService _gameService;
    private readonly UserInfo _userInfo;
    private readonly PlayerService _playerService;

    public Setup(MonopolyGameService gameService, UserInfo userInfo, PlayerService playerService)
    {
        _gameService = gameService;
        _userInfo = userInfo;
        _playerService = playerService;
    }

    public class TurnOrderViewModel
    {
        [DisplayName("Player")]
        public int PlayerId { get; set; }
        public int GamePlayerId { get; set; }
        [Required]
        public int Order { get; set; }
        [Required]
        [DisplayName("First Dice")]
        [Range(1, 6)]
        public int Dice1 { get; set; }
        [Required]
        [DisplayName("Second Dice")]
        [Range(1, 6)]
        public int Dice2 { get; set; }
    }
    
    public MonopolyGame Game { get; set; }
    public List<SelectListItem> PlayerOrders { get; set; } = [];
    [BindProperty] public List<TurnOrderViewModel> Input { get; set; } = [];

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
        
        var i = 1;
        foreach (var p in Game.Players)
        {
            Input.Add(new TurnOrderViewModel
            {
                PlayerId = p.Id,
                GamePlayerId = p.GamePid,
                Order = i,
                Dice1 = 0,
                Dice2 = 0
            });
            i++;
        }

        return Page();
    }

    public async Task<IActionResult> OnPost(int id)
    {
        var redirect = await SetupPage(id);
        if (redirect != null) return redirect;
        
        if (!ModelState.IsValid) return Page();

        if (Input.Select(turn => Input.Any(i => 
                i.Order == turn.Order && i.PlayerId != turn.PlayerId))
            .Any(dupeOrder => dupeOrder))
        {
            ModelState.AddModelError("Input", "The same player cannot have the same order as another player");
            return Page();
        }

        var result = await _playerService.SetupPlayerTurnOrders(Input
            .Select(i => (i.PlayerId, i.GamePlayerId, i.Order, i.Dice1, i.Dice2)).ToList(), id);
        if(result.IsValid) return RedirectToPage(nameof(Play), new {id});
        
        ModelState.AddModelError(result.ErrorKey, result.ErrorMsg);
        return Page();
    }
}