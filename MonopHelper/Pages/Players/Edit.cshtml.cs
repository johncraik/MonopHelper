using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonopolyCL.Models.Players.DataModel;
using MonopolyCL.Models.Validation;
using MonopolyCL.Services.Players;

namespace MonopHelper.Pages.Players;

public class Edit : PageModel
{
    private readonly PlayerService _playerService;
    private PlayerDM Player { get; set; }
    public bool Adding = false;

    public Edit(PlayerService playerService)
    {
        _playerService = playerService;
    }
    
    [Required]
    [BindProperty]
    public string Name { get; set; }

    public async Task<bool> SetupPage(int id)
    {
        if (Adding) return true;
        
        var player = await _playerService.GetPlayer(id);
        if (player == null) return false;

        Player = player;
        return true;
    }

    public void SetupAdding(int id)
    {
        if (id == default)
        {
            Adding = true;
        }
    }
    
    public async Task<IActionResult> OnGet(int id)
    {
        SetupAdding(id);
        var res = await SetupPage(id);
        if (!res) return RedirectToPage(nameof(Index));
        
        if (Adding) return Page();

        Name = Player.Name;
        return Page();
    }

    public async Task<IActionResult> OnPost(int id)
    {
        SetupAdding(id);
        var res = await SetupPage(id);
        if (!res) return RedirectToPage(nameof(Index));

        if (!ModelState.IsValid) return Page();

        ValidationResponse response;
        if (Adding)
        {
            response = await _playerService.TryAddPlayer(Name);
        }
        else
        {
            response = await _playerService.TryUpdatePlayer(id, Name);
        }

        if (response.IsValid) return RedirectToPage(nameof(Index));
        
        ModelState.AddModelError(response.ErrorKey, response.ErrorMsg);
        return Page();
    }
}