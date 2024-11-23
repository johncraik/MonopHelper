using Microsoft.AspNetCore.Mvc;
using MonopHelper.Models.ViewModels;
using MonopHelper.Services;

namespace MonopHelper.Controllers;

public class GameController : Controller
{
    private readonly GameService _gameManager;

    public GameController(GameService gameManager)
    {
        _gameManager = gameManager;
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
}