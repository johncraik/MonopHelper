using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonopHelper.Models;
using MonopHelper.Services;
using MonopHelper.Services.InGame;

namespace MonopHelper.Pages.InGame;

public class RepayLoan : PageModel
{
    private readonly LoanService _loanService;
    private readonly PlayerService _playerService;

    public RepayLoan(LoanService loanService, PlayerService playerService)
    {
        _loanService = loanService;
        _playerService = playerService;
    }
    
    public Player? Player { get; set; }
    public Loan? Loan { get; set; }
    public List<Loan> AllLoans { get; set; }
    [BindProperty]
    public bool RepayAll { get; set; }
    
    [BindProperty]
    public int PlayerId { get; set; }
    [BindProperty]
    public int GameId { get; set; }
    [BindProperty]
    public int LoanId { get; set; }
    [BindProperty]
    public int RepayAmount { get; set; }
    
    public async Task<IActionResult> OnGet(int id, bool all = false)
    {
        var success = await PopulateData(id, all);
        if(!success) return RedirectToPage(nameof(NotFound));
        
        //Return to game if repaid, or all loans is empty (when repaying all):
        if(Loan?.Repaid ?? false) return RedirectToPage($"/InGame/{nameof(Index)}", new { id = Player?.GameId });
        if(all && AllLoans.Count == 0) return RedirectToPage($"/InGame/{nameof(Index)}", new { id = Player?.GameId });

        RepayAll = all;
        return Page();
    }
    

    public async Task<IActionResult> OnPostOnePass()
    {
        if (RepayAll)
        {
            await _loanService.RepayAll(PlayerId, RepayAmount, 1);
        }
        else
        {
            await Repay();
        }
        
        return RedirectToPage($"/InGame/{nameof(Index)}", new { id = GameId });
    }
    
    public async Task<IActionResult> OnPostTwoPass()
    {
        if (RepayAll)
        {
            await _loanService.RepayAll(PlayerId, RepayAmount, 2);
        }
        else
        {
            await Repay();
        }
        
        return RedirectToPage($"/InGame/{nameof(Index)}", new { id = GameId });
    }
    
    public async Task<IActionResult> OnPostThreePass()
    {
        if (RepayAll)
        {
            await _loanService.RepayAll(PlayerId, RepayAmount, 3);
        }
        else
        {
            await Repay();
        }
        
        return RedirectToPage($"/InGame/{nameof(Index)}", new { id = GameId });
    }
    
    public async Task<IActionResult> OnPostCustom()
    {
        if (RepayAll)
        {
            await _loanService.RepayAll(PlayerId, RepayAmount, 0);
        }
        else
        {
            await Repay();
        }
        
        return RedirectToPage($"/InGame/{nameof(Index)}", new { id = GameId });
    }

    
    private async Task Repay()
    {
        await _loanService.Repay(LoanId, RepayAmount);
    }
    
    private async Task<bool> PopulateData(int id, bool all = false)
    {
        if (all)
        {
            AllLoans = await _loanService.GetPlayerLoans(id, false);
        }
        else
        {
            Loan = await _loanService.FindLoan(id);
            if (Loan == null) return false;
        }
        
        Player = await _playerService.GetPlayer(all ? id : Loan?.PlayerId ?? 0);
        return Player != null;
    }
}