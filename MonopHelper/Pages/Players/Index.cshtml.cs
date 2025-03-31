using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonopHelper.Pages.TurnBased;
using MonopolyCL.Models.Players.DataModel;
using MonopolyCL.Services.Players;

namespace MonopHelper.Pages.Players;

[Authorize]
public class Index : PageModel
{
    private readonly PlayerService _playerService;

    public Index(PlayerService playerService)
    {
        _playerService = playerService;
    }
    
    public List<PlayerDM> Players { get; set; }
    
    public async Task OnGet()
    {
        Players = await _playerService.GetPlayers();
    }
}