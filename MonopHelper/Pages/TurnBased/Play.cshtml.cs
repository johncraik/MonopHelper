using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonopolyCL.Models.Game;
using MonopolyCL.Models.Players;
using MonopolyCL.Services.Game;
using MonopolyCL.Services.Players;
using MonopolyCL.Services.Properties;

namespace MonopHelper.Pages.TurnBased;

[Authorize]
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

    public class PayReservationViewModel
    {
        [Required]
        public int PlayerId { get; set; }
        [Required]
        public string Amounts { get; set; }
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
    [BindProperty]
    public PayReservationViewModel PayReservationInput { get; set; } = new PayReservationViewModel();
    public List<SelectListItem> ReservationProperties { get; set; }
    
    public MonopolyGame Game { get; set; }
    private TurnOrder TurnOrder { get; set; }
    public GameAlert Alert { get; set; }
    public IPlayer? CurrentPlayer { get; set; }
    [BindProperty(SupportsGet = true)]
    public int SwitchPlayerId { get; set; }

    public bool SwitchView { get; set; }

    public async Task<IActionResult?> SetupPage(int id, int switchId = 0)
    {
        var game = await _gameService.FetchGame(id);
        if (game == null) return RedirectToPage(nameof(Index));

        if (game.Players.Count < 2) return RedirectToPage(nameof(Win), new {id});
        
        var turnOrder = await _gameService.GetGameTurnOrder(game.Game.Id);
        if(turnOrder == null) return RedirectToPage(nameof(Index));
        if (!turnOrder.IsSetup) return RedirectToPage(nameof(Setup), new {id = game.Game.Id});

        TurnOrder = turnOrder;
        CurrentPlayer = game.Players.FirstOrDefault(p => p.Id == turnOrder.CurrentTurn);
        Game = game;

        if (SwitchPlayerId > 0 || switchId > 0)
        {
            if (switchId > 0) SwitchPlayerId = switchId;
            var player = Game.Players.FirstOrDefault(p => p.Id == SwitchPlayerId);
            if (player != null)
            {
                SwitchView = true;
                CurrentPlayer = player;
            }
        }
        
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
    
    public async Task<IActionResult> OnGet(int id, int switchId = 0)
    {
        var res = await SetupPage(id, switchId);
        return res ?? Page();
    }

    public async Task<IActionResult> OnPostEnd(int id)
    {
        var res = await _playerService.EndTurn(id);
        return res ? RedirectToPage(nameof(Play), new {id}) : RedirectToPage(nameof(Index));
    }

    public IActionResult OnPostReturn(int id) => RedirectToPage(nameof(Play), new { id });
    
    public async Task<IActionResult> OnPostFreeParking(int id)
    {
        var redirect = await SetupPage(id);
        if (redirect != null) return redirect;
        ModelState.Remove("Amounts");
        if (!ModelState.IsValid) return Page();

        var res = await _propertyService.HandInProperty(FreeParkingInput.Id, FreeParkingInput.PlayerId);
        return res ? RedirectToPage(nameof(Play), new { id, switchId = SwitchPlayerId }) : Page();
    }

    public async Task<IActionResult> OnPostMortgage(int id)
    {
        var redirect = await SetupPage(id);
        if (redirect != null) return redirect;
        ModelState.Remove("Amounts");
        if (!ModelState.IsValid) return Page();

        var res = await _propertyService.MortgageProperty(MortgageInput.Id, MortgageInput.PlayerId);
        return res ? RedirectToPage(nameof(Play), new { id, switchId = SwitchPlayerId }) : Page();
    }

    public async Task<IActionResult> OnPostPayLoan(int id)
    {
        var redirect = await SetupPage(id);
        if (redirect != null) return redirect;
        ModelState.Remove("Amounts");
        if (!ModelState.IsValid) return Page();

        await _playerService.PayLoans(PayLoanInput.PlayerId, PayLoanInput.Percentage, PayLoanInput.TotalAmount);
        return RedirectToPage(nameof(Play), new { id, switchId = SwitchPlayerId });
    }

    public async Task<IActionResult> OnPostNewLoan(int id)
    {
        var redirect = await SetupPage(id);
        if (redirect != null) return redirect;
        ModelState.Remove("Amounts");
        if (!ModelState.IsValid) return Page();

        var res = await _playerService.NewLoan(NewLoanInput.PlayerId, NewLoanInput.Amount);
        if (res.IsValid) return RedirectToPage(nameof(Play), new { id, switchId = SwitchPlayerId });
        
        ModelState.AddModelError(res.ErrorKey, res.ErrorMsg);
        return Page();
    }

    public async Task<IActionResult> OnPostReservation(int id)
    {
        var redirect = await SetupPage(id);
        if (redirect != null) return redirect;
        ModelState.Remove("Amounts");
        if (!ModelState.IsValid) return Page();

        var res = await _propertyService.ReserveProperty(ReservationInput.Id, ReservationInput.PlayerId);
        return res ? RedirectToPage(nameof(Play), new { id, switchId = SwitchPlayerId }) : Page();
    }

    public async Task<IActionResult> OnPostPayReservation(int id)
    {
        var redirect = await SetupPage(id);
        if (redirect != null) return redirect;
        if (!ModelState.IsValid) return Page();

        var res = await _propertyService.PayReservationFee(PayReservationInput.Amounts, PayReservationInput.PlayerId);
        return res ? RedirectToPage(nameof(Play), new { id, switchId = SwitchPlayerId }) : Page();
    }

    public async Task<IActionResult> OnPostBankrupt(int id)
    {
        var redirect = await SetupPage(id);
        if (redirect != null) return redirect;
        ModelState.Remove("Amounts");
        if (!ModelState.IsValid) return Page();

        var res = await _playerService.DeclareBankruptcy(CurrentPlayer, TurnOrder);
        return res ? RedirectToPage(nameof(Play), new { id }) : Page();
    }

    public async Task<IActionResult> OnPostDraw(int id)
    {
        var redirect = await SetupPage(id);
        if (redirect != null) return redirect;
        
        ModelState.Remove("Amounts");
        if (!ModelState.IsValid) return Page();

        await _gameService.DeleteGame(id);
        return RedirectToPage(nameof(Index));
    }
}