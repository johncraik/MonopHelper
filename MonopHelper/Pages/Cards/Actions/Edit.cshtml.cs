using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonopolyCL.Models.Cards;
using MonopolyCL.Models.Cards.CardActions;
using MonopolyCL.Models.Cards.CardActions.EventActions;
using MonopolyCL.Models.Cards.CardActions.MoveActions;
using MonopolyCL.Services.Cards;

namespace MonopHelper.Pages.Cards.Actions;

public class Edit : PageModel
{
    private readonly CardActionService _cardActionService;
    private readonly CardService _cardService;

    public Edit(CardActionService cardActionService,
        CardService cardService)
    {
        _cardActionService = cardActionService;
        _cardService = cardService;
    }
    
    [BindProperty]
    public PayAction Pay { get; set; }
    [BindProperty]
    public PropertyAction Property { get; set; }
    [BindProperty]
    public DiceAction Dice { get; set; }
    [BindProperty]
    public ResetAction Reset { get; set; }
    [BindProperty]
    public TakeCardAction TakeCard { get; set; }
    public EventBaseAction Event { get; set; }
    
    public CardActions Type { get; set; }
    public int Group { get; set; }
    public ActionConfig? Config { get; set; }
    public bool AddingConfig { get; set; }
    public bool AddingAction { get; set; }
    public Card? Card { get; set; }
    public List<SelectListItem> CardTypes { get; set; } = [];

    public async Task<bool> Setup(int cardId, int id, int group, int action)
    {
        //Validate and set action type:
        if (!Enum.IsDefined(typeof(CardActions), action)) return false;
        Type = (CardActions)action;

        //Validate and set config and groups:
        (Config, var invalid) = await _cardActionService.GetActionConfig(cardId);
        if (Config != null)
        {
            var groupCount = Config.GetGroups().Length;
            if (group > 0 && group > groupCount) return false;
        }
        else if(group > 0 || invalid)
        {
            //Invalid group OR invalid config:
            return false;
        }
        else
        {
            //No config, so adding:
            AddingConfig = true;
        }

        Group = group;
        
        if (id <= 0) AddingAction = true;
        
        if (AddingConfig)
        {
            Card = await _cardService.FindCard(cardId);
            if (Card == null) return false;
        }
        else
        {
            Card = Config?.Card;
            if (Card == null) return false;
        }
        
        return true;
    }

    public async Task<bool> SetupAction(int id)
    {
        switch (Type)
        {
            case CardActions.MOVE:
                // if (AddingAction)
                // {
                //     Move = new MoveAction
                //     {
                //         Group = Group
                //     };
                // }
                // else
                // {
                //     var move = _cardActionService.GetAction(Card!.Id, Group, id, Type);
                //     if (move == null) return false;
                //     Move = (MoveAction)move;
                //     Move.Group = Group;
                // }
                break;
            case CardActions.PAY:
                if (AddingAction)
                {
                    Pay = new PayAction
                    {
                        Group = Group
                    };
                }
                else
                {
                    var pay = _cardActionService.GetAction(Card!.Id, Group, id, Type);
                    if (pay == null) return false;
                    Pay = (PayAction)pay;
                    Pay.Group = Group;
                }
                break;
            case CardActions.PROPERTY:
                if (AddingAction)
                {
                    Property = new PropertyAction
                    {
                        Group = Group
                    };
                }
                else
                {
                    var property = _cardActionService.GetAction(Card!.Id, Group, id, Type);
                    if (property == null) return false;
                    Property = (PropertyAction)property;
                    Property.Group = Group;
                }
                break;
            case CardActions.DICE:
                if (AddingAction)
                {
                    Dice = new DiceAction
                    {
                        Group = Group
                    };
                }
                else
                {
                    var dice = _cardActionService.GetAction(Card!.Id, Group, id, Type);
                    if (dice == null) return false;
                    Dice = (DiceAction)dice;
                    Dice.Group = Group;
                }
                break;
            case CardActions.RESET:
                if (AddingAction)
                {
                    Reset = new ResetAction
                    {
                        Group = Group
                    };
                }
                else
                {
                    var reset = _cardActionService.GetAction(Card!.Id, Group, id, Type);
                    if (reset == null) return false;
                    Reset = (ResetAction)reset;
                    Reset.Group = Group;
                }
                break;
            case CardActions.TAKE_CARD:
                CardTypes = (await _cardService.GetCardTypes())
                    .Select(t => new SelectListItem
                    {
                        Text = t.Name,
                        Value = t.Id.ToString()
                    }).ToList();
                
                if (AddingAction)
                {
                    TakeCard = new TakeCardAction
                    {
                        Group = Group
                    };
                }
                else
                {
                    var takeCard = _cardActionService.GetAction(Card!.Id, Group, id, Type);
                    if (takeCard == null) return false;
                    TakeCard = (TakeCardAction)takeCard;
                    TakeCard.Group = Group;
                }
                break;
            case CardActions.EVENT:
                if (AddingAction)
                {
                    Event = new EventBaseAction
                    {
                        Group = Group
                    };
                }
                else
                {
                    var eventAction = _cardActionService.GetAction(Card!.Id, Group, id, Type);
                    if (eventAction == null) return false;
                    Event = (EventBaseAction)eventAction;
                    Event.Group = Group;
                }
                break;
            default:
                return false;
        }

        return true;
    }
    
    public async Task<IActionResult> OnGet(int card, int id, int group, int action)
    {
        var res = await Setup(card, id, group, action);
        if(!res) RedirectToPage(nameof(Index), new {id = card});

        res = await SetupAction(id);
        return res ? Page() : RedirectToPage(nameof(Index), new {id = card});
    }

    public async Task<IActionResult> OnPost(int card, int id, int group, int action)
    {
        var res = await Setup(card, id, group, action);
        if(!res) RedirectToPage(nameof(Index), new {id = card});

        ICardActionModel? model;
        switch (Type)
        {
            case CardActions.PAY:
                foreach (var key in ModelState.Keys.Where(key => !key.Contains("Pay")))
                {
                    ModelState.Remove(key);
                }
                
                model = Pay;
                break;
            case CardActions.PROPERTY:
                foreach (var key in ModelState.Keys.Where(key => !key.Contains("Property")))
                {
                    ModelState.Remove(key);
                }
                
                model = Property;
                break;
            case CardActions.DICE:
                foreach (var key in ModelState.Keys.Where(key => !key.Contains("Dice")))
                {
                    ModelState.Remove(key);
                }
                
                model = Dice;
                break;
            case CardActions.RESET:
                foreach (var key in ModelState.Keys.Where(key => !key.Contains("Reset")))
                {
                    ModelState.Remove(key);
                }
                
                model = Reset;
                break;
            case CardActions.TAKE_CARD:
                foreach (var key in ModelState.Keys.Where(key => !key.Contains("TakeCard")))
                {
                    ModelState.Remove(key);
                }
                
                model = TakeCard;
                break;
            case CardActions.MOVE:
            case CardActions.EVENT:
            default:
                return Page();
        }
        
        model.Validate(ModelState);
        if (!ModelState.IsValid) return Page();
        
        if (AddingAction)
        {
            await _cardActionService.TryCreateAction(Config, model, card);
        }
        else
        {
            await _cardActionService.TryUpdateAction(model, card);
        }
        
        return RedirectToPage(nameof(Index), new {id = card});
    }
}