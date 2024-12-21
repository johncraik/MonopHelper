using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonopHelper.Models;
using MonopHelper.Services;
using MonopHelper.Services.InGame;

namespace MonopHelper.Pages.InGame;

[Authorize]
public class NewLoan : PageModel
{
    private readonly LoanService _loanService;
    private readonly PlayerService _playerService;

    public NewLoan(LoanService loanService, PlayerService playerService)
    {
        _loanService = loanService;
        _playerService = playerService;
    }
    
    public Player? Player { get; set; }
    
    [BindProperty]
    [Required]
    public Loan Loan { get; set; }
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
        var success = await _loanService.CreateLoan(Loan);
        if (success) return RedirectToPage($"/InGame/{nameof(Index)}", new { id = GameId });
        
        success = await PopulateData(Loan.PlayerId);
        if(!success) return RedirectToPage(nameof(NotFound));
        
        //Add error:
        ModelState.AddModelError("Loan.Amount", "You can only have a maximum of 3 unpaid loans at one time!");
        return Page();

    }
    
    private async Task<bool> PopulateData(int playerId)
    {
        Player = await _playerService.GetPlayer(playerId);
        return Player != null;
    }
}