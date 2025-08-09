using Microsoft.AspNetCore.Mvc;
using MonopolyCL.Models.Cards.CardActions;
using MonopolyCL.Models.Cards.CardActions.EventActions;
using MonopolyCL.Models.Cards.CardActions.MoveActions;
using MonopolyCL.Services.Cards;
using Index = MonopHelper.Pages.Cards.Actions.Index;

namespace MonopHelper.Controllers;

public class CardActionController : Controller
{
    private readonly CardActionService _cardActionService;
    private const string PartialBaseUrl = "../Cards/Actions/Partials/Edit/";

    public CardActionController(CardActionService cardActionService)
    {
        _cardActionService = cardActionService;
    }

    [HttpGet]
    public IActionResult? GetEditEventPartial(int cardId, int groupId, int actionId, EventType eventType)
    {
        var action = _cardActionService.GetAction(cardId, groupId, actionId, CardActions.EVENT);
        EventBaseAction? eventAction = null;
        if (action != null)
        {
            eventAction = (EventBaseAction)action;
        }

        var adding = true;
        switch (eventType)
        {
            case EventType.TURNS:
                var turns = new TurnsEvent();
                if (action != null)
                {
                    turns = (TurnsEvent)action;
                    adding = false;
                }

                turns.Group = groupId;
                
                return PartialView($"{PartialBaseUrl}EventActions/_EditTurnsEvent", 
                    new ActionViewModel<TurnsEvent>(turns, cardId, adding));
            case EventType.FORCE_MORTGAGE:
                var mortgage = new ForceMortgageEvent();
                if (action != null)
                {
                    mortgage = (ForceMortgageEvent)action;
                    adding = false;
                }

                mortgage.Group = groupId;
                
                return PartialView($"{PartialBaseUrl}EventActions/_EditMortgageEvent", 
                    new ActionViewModel<ForceMortgageEvent>(mortgage, cardId, adding)); 
            case EventType.UNIQUE_SPACE:
                var space = new SpaceEvent();
                if (action != null)
                {
                    space = (SpaceEvent)action;
                    adding = false;
                }

                space.Group = groupId;
                
                return PartialView($"{PartialBaseUrl}EventActions/_EditSpaceEvent", 
                    new ActionViewModel<SpaceEvent>(space, cardId, adding)); 
            case EventType.GO:
                var go = new GoEvent();
                if (action != null)
                {
                    go = (GoEvent)action;
                    adding = false;
                }

                go.Group = groupId;
                
                return PartialView($"{PartialBaseUrl}EventActions/_EditGoEvent", 
                    new ActionViewModel<GoEvent>(go, cardId, adding)); 
            case EventType.FREE_PARKING:
                var fp = new FreeParkingEvent();
                if (action != null)
                {
                    fp = (FreeParkingEvent)action;
                    adding = false;
                }

                fp.Group = groupId;
                
                return PartialView($"{PartialBaseUrl}EventActions/_EditFpEvent", 
                    new ActionViewModel<FreeParkingEvent>(fp, cardId, adding)); 
            case EventType.JAIL:
                var jail = new JailEvent();
                if (action != null)
                {
                    jail = (JailEvent)action;
                    adding = false;
                }

                jail.Group = groupId;
                
                return PartialView($"{PartialBaseUrl}EventActions/_EditJailEvent", 
                    new ActionViewModel<JailEvent>(jail, cardId, adding)); 
            case EventType.NONE:
            default:
                return PartialView("../Cards/Actions/Partials/Edit/EventActions/_EditTurnsEvent", null);
        }
    }

    private async Task EditComplexAction(ICardActionModel? action, int cardId)
    {
        if (action == null)
        {
            ModelState.AddModelError("Action", "Action is null.");
            return;
        }
        
        action.Validate(ModelState);
        if (!ModelState.IsValid) return;
        
        if (action.ActionId == 0)
        {
            var (config, invalid) = await _cardActionService.GetActionConfig(cardId);
            if (invalid) return;
            
            await _cardActionService.TryCreateAction(config, action, cardId);
        }
        else
        {
            await _cardActionService.TryUpdateAction(action, cardId);
        }
    }
    

    [HttpPost]
    public async Task<IActionResult> EditTurnsEvent(ActionViewModel<TurnsEvent> model)
    {
        await EditComplexAction(model.Action, model.CardId);
        return Redirect($"/Cards/Actions/Index/{model.CardId}");
    }

    [HttpPost]
    public async Task<IActionResult> EditMortgageEvent(ActionViewModel<ForceMortgageEvent> model)
    {
        await EditComplexAction(model.Action, model.CardId);
        return Redirect($"/Cards/Actions/Index/{model.CardId}");
    }
    
    [HttpPost]
    public async Task<IActionResult> EditSpaceEvent(ActionViewModel<SpaceEvent> model)
    {
        await EditComplexAction(model.Action, model.CardId);
        return Redirect($"/Cards/Actions/Index/{model.CardId}");
    }
    
    [HttpPost]
    public async Task<IActionResult> EditGoEvent(ActionViewModel<GoEvent> model)
    {
        await EditComplexAction(model.Action, model.CardId);
        return Redirect($"/Cards/Actions/Index/{model.CardId}");
    }

    [HttpPost]
    public async Task<IActionResult> EditFpEvent(ActionViewModel<FreeParkingEvent> model)
    {
        await EditComplexAction(model.Action, model.CardId);
        return Redirect($"/Cards/Actions/Index/{model.CardId}");
    }
    
    [HttpPost]
    public async Task<IActionResult> EditJailEvent(ActionViewModel<JailEvent> model)
    {
        await EditComplexAction(model.Action, model.CardId);
        return Redirect($"/Cards/Actions/Index/{model.CardId}");
    }
}