using Microsoft.AspNetCore.Mvc;
using MonopHelper.Models.ViewModels;
using MonopHelper.Services;
using MonopHelper.Services.InGame;

namespace MonopHelper.Controllers;

public class GameController : Controller
{
    private readonly GameService _gameManager;
    private readonly PlayerService _playerService;

    public GameController(GameService gameManager, PlayerService playerService)
    {
        _gameManager = gameManager;
        _playerService = playerService;
    }
    
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult NewGamePlayersPartial(string players)
    {
        var model = new NewGamePlayers(players);
        return PartialView("CreateGame/_NewGamePlayers", model);
    }

    public async Task<IActionResult> LoadGamesPartial()
    {
        var model = await _gameManager.FetchGames();
        return PartialView("CreateGame/_LoadGames", model);
    }

    [HttpPost]
    public async Task<int> LeaveJail(int id)
    {
        return await _playerService.LeaveJail(id);
    }

    [HttpPost]
    public async Task<int> SetNumber(int id, int d1, int d2)
    {
        return await _playerService.SetNumber(id, (byte)d1, (byte)d2);
    }
}