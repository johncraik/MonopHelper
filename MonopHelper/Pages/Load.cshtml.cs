using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonopHelper.Models;
using MonopHelper.Models.ViewModels;
using MonopHelper.Services;

namespace MonopHelper.Pages;

[Authorize]
public class Load : PageModel
{
    private readonly GameService _gameManager;

    public Load(GameService gameManager)
    {
        _gameManager = gameManager;
    }
    
    public List<LoadGame> LoadModel { get; set; }
    
    public async Task<IActionResult> OnGet()
    {
        LoadModel = await _gameManager.FetchGames();
        return Page();
    }
}