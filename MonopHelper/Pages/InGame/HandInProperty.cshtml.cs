using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonopHelper.Models;
using MonopHelper.Models.Enums;
using MonopHelper.Services;
using MonopHelper.Services.InGame;

namespace MonopHelper.Pages.InGame;

[Authorize]
public class HandInProperty : PageModel
{
    private readonly PropertyService _propertyService;
    private readonly GameService _gameManager;
    private readonly PlayerService _playerService;

    public HandInProperty(PropertyService propertyService, GameService gameManager, PlayerService playerService)
    {
        _propertyService = propertyService;
        _gameManager = gameManager;
        _playerService = playerService;
    }
    
    public Player? Player { get; set; }
    public List<SelectListItem> UnusedCols { get; set; }
    
    [BindProperty]
    [Required(ErrorMessage = "You must select a property type.")]
    [DisplayName("Select a Property Type")]
    public string SelectedCol { get; set; }
    [BindProperty]
    public int PlayerId { get; set; }
    [BindProperty]
    public int GameId { get; set; }
    
    
    public async Task<IActionResult> OnGet(int id)
    {
        var success = await PopulateData(id);
        if(!success) return RedirectToPage(nameof(NotFound));
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            var success = await PopulateData(PlayerId);
            if(!success) return RedirectToPage(nameof(NotFound));
            return Page();
        }

        var col = _propertyService.FindPropertyColour(SelectedCol);
        if (col == null)
        {
            ModelState["SelectedCol"]?.Errors.Add("Invalid property type selected! Please try again.");
            var success = await PopulateData(PlayerId);
            if(!success) return RedirectToPage(nameof(NotFound));
            return Page();
        }

        await _propertyService.AddProperty((PropertyCol)col, PlayerId);
        return RedirectToPage($"/InGame/{nameof(Index)}", new { id = GameId });
    }

    private async Task<bool> PopulateData(int playerId)
    {
        Player = await _playerService.GetPlayer(playerId);
        if (Player == null) return false;

        var unusedCols = await _propertyService.GetPlayerUnusedColours(playerId);
        UnusedCols = unusedCols.Select(uc => new SelectListItem
        {
            Text = PropCol.GetPropertyString(uc),
            Value = ((int)uc).ToString()
        }).ToList();

        return true;
    }
}