using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonopHelper.Models.ViewModels;
using MonopHelper.Services;
using Index = MonopHelper.Pages.InGame.Index;

namespace MonopHelper.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly GameService _gameManager;

    public IndexModel(ILogger<IndexModel> logger, GameService gameManager)
    {
        _logger = logger;
        _gameManager = gameManager;
    }

    public NewGamePlayers NoPlayers { get; set; }
    
    [BindProperty]
    [DisplayName("Name")]
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
        var playersAdded = await _gameManager.AddPlayers(p, gameId);

        return !playersAdded ? InvalidPost(p) : RedirectToPage($"/InGame/{nameof(Index)}", new {id = gameId});
    }

    private IActionResult InvalidPost(string? p)
    {
        NoPlayers = new NewGamePlayers(p);
        return Page();
    }
}