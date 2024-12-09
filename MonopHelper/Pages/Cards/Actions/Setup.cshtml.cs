using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonopHelper.Services.Cards;
using MonopolyCL.Extensions;
using MonopolyCL.Models.Cards;
using MonopolyCL.Models.Cards.Actions;
using MonopolyCL.Models.Cards.Enums;
using MonopolyCL.Models.Properties.Enums;
using MonopolyCL.Services.Cards;

namespace MonopHelper.Pages.Cards.Actions;

public class Setup : PageModel
{
    private readonly CardService _cardService;
    private readonly CardActionsService _cardActionsService;

    public Setup(CardService cardService, CardActionsService cardActionsService)
    {
        _cardService = cardService;
        _cardActionsService = cardActionsService;
    }
    
    public Card Card { get; set; }
    public List<SelectListItem> SetList { get; set; }
    public List<SelectListItem> PayTypeList { get; set; }
    public List<SelectListItem> CardTypeList { get; set; }
    
    [BindProperty]
    public int CardId { get; set; }
    [BindProperty]
    public CARD_ACTION Action { get; set; }
    [BindProperty]
    public ActionModel ActionModel { get; set; }

    public async Task<bool> SetupPage(int id)
    {
        var card = await _cardService.FindCard(id);
        if (card == null) return false;

        //Return false if action already exists:
        var (cardAction, action) = await _cardActionsService.GetCardAction(card.Id);
        if (cardAction != null || action != null) return false;
        
        Card = card;
        SetList = SetExtensions.GetSetList().Select(s => new SelectListItem
        {
            Text = s.GetDisplayName(),
            Value = ((int)s).ToString()
        }).ToList();
        PayTypeList = PayPlayerExtensions.GetPayTypeList().Select(p => new SelectListItem
        {
            Text = p.GetDisplayName(),
            Value = ((int)p).ToString()
        }).ToList();
        CardTypeList = (await _cardService.GetCardTypes()).Select(t => new SelectListItem
        {
            Text = t.Name,
            Value = t.Id.ToString()
        }).ToList();
        
        return true;
    }
    
    public async Task<IActionResult> OnGet(int id, int action)
    {
        var success = await SetupPage(id);
        if (!success) return new UnauthorizedResult();
        
        Action = (CARD_ACTION)action;
        ActionModel = new ActionModel();
        ActionModel.SetModel(Action);
        
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        var success = await SetupPage(CardId);
        if (!success) return new UnauthorizedResult();

        ModelState.Remove("ActionModel.AdvanceAction.Colour");
        ModelState.Remove("ActionModel.ChoiceAction.CardType");
        if (!ModelState.IsValid) return Page();
        
        var res = Action switch
        {
            CARD_ACTION.ADVANCE => await _cardActionsService.AddAdvanceAction(Card.Id, 
                ActionModel.AdvanceAction!.AdvanceIndex, 
                ActionModel.AdvanceAction!.Set, 
                ActionModel.AdvanceAction.ClaimGo),
            CARD_ACTION.MOVE => await _cardActionsService.AddMoveAction(CardId, 
                ActionModel.MoveAction!.MoveAmount, 
                ActionModel.MoveAction!.IsForward),
            CARD_ACTION.KEEP_CARD => await _cardActionsService.AddKeepAction(CardId),
            CARD_ACTION.CHOICE => await _cardActionsService.AddChoiceAction(CardId, ActionModel.ChoiceAction!.CardTypeId),
            CARD_ACTION.PAY_PLAYER => await _cardActionsService.AddPayPlayerAction(CardId, 
                ActionModel.PayPlayerAction!.PayToType),
            CARD_ACTION.STREET_REPAIRS => await _cardActionsService.AddStreetRepairsAction(CardId, 
                ActionModel.StreetRepairsAction!.HouseCost, 
                ActionModel.StreetRepairsAction!.HotelCost),
            _ => false
        };

        if (!res) return Page();

        return RedirectToPage($"/Cards/Actions/{nameof(Index)}", new {id = CardId});
    }
}