using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonopHelper.Models.ViewModels;
using MonopHelper.Services;

namespace MonopHelper.Pages;

public class Create : PageModel
{
    private readonly GameService _gameManager;

    public Create(GameService gameManager)
    {
        _gameManager = gameManager;
    }
    
    public NewGamePlayers NoPlayers { get; set; }
    
    [BindProperty]
    [DisplayName("Game Name")]
    [Required(ErrorMessage = "Please give this game a name for reference.")]
    [MaxLength(50, ErrorMessage = "The name cannot be longer than 50 characters.")]
    public string GameName { get; set; }
    [BindProperty]
    public string Players { get; set; }

    public IActionResult OnGet()
    {
        NoPlayers = new NewGamePlayers(null);
        return Page();
    }
    
    public async Task<IActionResult> OnPost()
    {
        var p = Players == "" ? null : Players;
        if (!ModelState.IsValid) return InvalidPost(p);
        
        var gameId = await _gameManager.CreateGame(GameName);
        var (playersAdded, msg) = await _gameManager.AddPlayers(p, gameId);

        return !playersAdded ? InvalidPost(p, msg) 
            : RedirectToPage($"/InGame/{nameof(Index)}", new {id = gameId});
    }

    private IActionResult InvalidPost(string? p, string noPlayersError = "")
    {
        if(noPlayersError != "") ModelState["Players"]?.Errors.Add(noPlayersError);
        
        NoPlayers = new NewGamePlayers(p);
        return Page();
    }
}