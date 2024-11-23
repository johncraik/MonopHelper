using Microsoft.AspNetCore.Mvc;
using MonopHelper.Models.ViewModels;

namespace MonopHelper.Controllers;

public class GameController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult NewGamePlayersPartial(string players)
    {
        var model = new NewGamePlayers(players);
        return PartialView("CreateGame/_NewGamePlayers", model);
    }
}