using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonopHelper.Services.Cards;
using MonopolyCL.Models.Cards;
using MonopolyCL.Models.Cards.Actions;
using MonopolyCL.Models.Cards.Enums;
using MonopolyCL.Services.Cards;

namespace MonopHelper.Pages.Cards.Actions;

public class Index : PageModel
{
    private readonly CardService _cardService;
    private readonly CardActionsService _cardActionsService;

    public Index(CardService cardService, CardActionsService cardActionsService)
    {
        _cardService = cardService;
        _cardActionsService = cardActionsService;
    }
    
    public Card Card { get; set; }
    [BindProperty]
    public int CardId { get; set; }
    
    public string ActionCol { get; set; }
    public CardAction? CardAction { get; set; }
    public ActionModel? Action { get; set; }

    public async Task<bool> SetupPage(int id)
    {
        var card = await _cardService.FindCard(id);
        if (card == null) return false;

        var (cardAction, action) = await _cardActionsService.GetCardAction(card.Id);
        if (cardAction != null)
        {
            CardAction = cardAction;
            if (action != null)
            {
                Action = new ActionModel();
                Action.SetModel(cardAction.Action, action);

                ActionCol = cardAction.Action.GetActionColour();
            }
        }
        
        if(string.IsNullOrEmpty(ActionCol)) ActionCol = "bg-danger text-white";
        
        Card = card;
        return true;
    }
    
    public async Task<IActionResult> OnGet(int id)
    {
        var success = await SetupPage(id);
        if (!success) return new UnauthorizedResult();
        
        return Page();
    }
}