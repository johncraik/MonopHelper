using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonopolyCL.Models.Game;
using MonopolyCL.Models.Players;
using MonopolyCL.Services.Game;
using MonopolyCL.Services.Players;
using MonopolyCL.Services.Properties;

namespace MonopHelper.Pages.TurnBased;

public class Play : PageModel
{
    private readonly MonopolyGameService _gameService;
    private readonly PlayerService _playerService;
    private readonly PropertyService _propertyService;

    public Play(MonopolyGameService gameService, PlayerService playerService, PropertyService propertyService)
    {
        _gameService = gameService;
        _playerService = playerService;
        _propertyService = propertyService;
    }
    
    public class FreeParkingViewModel
    {
        [DisplayName("Property")]
        [Required]
        public int Id { get; set; }
        [Required]
        public int PlayerId { get; set; }
    }

    public class MortgageViewModel
    {
        [DisplayName("Property")]
        [Required]
        public int Id { get; set; }
        [Required]
        public int PlayerId { get; set; }
    }
    
    public class PayLoans
    {
        [Required]
        public int PlayerId { get; set; }
        public byte? Percentage { get; set; }
        [DisplayName("Total Amount")]
        public uint? TotalAmount { get; set; }
    }

    public class NewLoan
    {
        [Required]
        public int PlayerId { get; set; }
        [DisplayName("Loan Amount")]
        [Required]
        public uint Amount { get; set; }
    }

    public class ReservationViewModel
    {
        [DisplayName("Property")]
        [Required]
        public int Id { get; set; }
        [Required]
        public int PlayerId { get; set; }
    }

    [BindProperty]
    public FreeParkingViewModel FreeParkingInput { get; set; } = new FreeParkingViewModel();
    public List<SelectListItem> FreeParkingProperties { get; set; }

    [BindProperty]
    public MortgageViewModel MortgageInput { get; set; } = new MortgageViewModel();
    public List<SelectListItem> MortgageProperties { get; set; }

    [BindProperty]
    public PayLoans PayLoanInput { get; set; } = new PayLoans();
    [BindProperty]

    public NewLoan NewLoanInput { get; set; } = new NewLoan();

    [BindProperty]
    public ReservationViewModel ReservationInput { get; set; } = new ReservationViewModel();
    public List<SelectListItem> ReservationProperties { get; set; }
    
    public MonopolyGame Game { get; set; }
    public GameAlert Alert { get; set; }
    public IPlayer? CurrentPlayer { get; set; }

    public async Task<IActionResult?> SetupPage(int id)
    {
        var game = await _gameService.FetchGame(id);
        if (game == null) return RedirectToPage(nameof(Index));
        
        var turnOrder = await _gameService.GetGameTurnOrder(game.Game.Id);
        if(turnOrder == null) return RedirectToPage(nameof(Index));
        if (!turnOrder.IsSetup) return RedirectToPage(nameof(Setup), new {id = game.Game.Id});
        
        CurrentPlayer = game.Players.FirstOrDefault(p => p.Id == turnOrder.CurrentTurn);
        Game = game;
        Alert = _gameService.GetGameAlert(CurrentPlayer);

        FreeParkingProperties = (await _propertyService.GetFreeParkingDropDown(CurrentPlayer?.GamePid ?? 0, id))
            .Select(p => new SelectListItem
            {
                Text = p.Txt,
                Value = p.Val
            }).ToList();
        MortgageProperties = (await _propertyService.GetMortgageDropdown(CurrentPlayer?.GamePid ?? 0, id))
            .Select(p => new SelectListItem
            {
                Text = p.Txt,
                Value = p.Val
            }).ToList();
        ReservationProperties = (await _propertyService.GetReservationDropdown(CurrentPlayer?.GamePid ?? 0, id))
            .Select(p => new SelectListItem
            {
                Text = p.Txt,
                Value = p.Val
            }).ToList();
        
        return null;
    }
    
    public async Task<IActionResult> OnGet(int id)
    {
        var res = await SetupPage(id);
        return res ?? Page();
    }

    public async Task<IActionResult> OnPostEnd(int id)
    {
        var res = await _playerService.EndTurn(id);
        return res ? RedirectToPage(nameof(Play), new {id}) : RedirectToPage(nameof(Index));
    }

    public async Task<IActionResult> OnPostFreeParking(int id)
    {
        var redirect = await SetupPage(id);
        if (redirect != null) return redirect;
        if (!ModelState.IsValid) return Page();

        var res = await _propertyService.HandInProperty(FreeParkingInput.Id, FreeParkingInput.PlayerId);
        return res ? RedirectToPage(nameof(Play), new { id }) : Page();
    }

    public async Task<IActionResult> OnPostMortgage(int id)
    {
        var redirect = await SetupPage(id);
        if (redirect != null) return redirect;
        if (!ModelState.IsValid) return Page();

        var res = await _propertyService.MortgageProperty(MortgageInput.Id, MortgageInput.PlayerId);
        return res ? RedirectToPage(nameof(Play), new { id }) : Page();
    }

    public async Task<IActionResult> OnPostPayLoan(int id)
    {
        var redirect = await SetupPage(id);
        if (redirect != null) return redirect;
        if (!ModelState.IsValid) return Page();

        await _playerService.PayLoans(PayLoanInput.PlayerId, PayLoanInput.Percentage, PayLoanInput.TotalAmount);
        return RedirectToPage(nameof(Play), new { id });
    }

    public async Task<IActionResult> OnPostNewLoan(int id)
    {
        var redirect = await SetupPage(id);
        if (redirect != null) return redirect;
        if (!ModelState.IsValid) return Page();

        var res = await _playerService.NewLoan(NewLoanInput.PlayerId, NewLoanInput.Amount);
        if (res.IsValid) return RedirectToPage(nameof(Play), new { id });
        
        ModelState.AddModelError(res.ErrorKey, res.ErrorMsg);
        return Page();
    }

    public async Task<IActionResult> OnPostReservation(int id)
    {
        var redirect = await SetupPage(id);
        if (redirect != null) return redirect;
        if (!ModelState.IsValid) return Page();

        var res = await _propertyService.ReserveProperty(ReservationInput.Id, ReservationInput.PlayerId);
        return res ? RedirectToPage(nameof(Play), new { id }) : Page();
    }
}