using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MonopolyCL.Models.Cards;
using MonopolyCL.Models.Cards.CardActions;
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
    public MoveAction Move { get; set; }
    [BindProperty]
    public PayAction Pay { get; set; }
    [BindProperty]
    public PropertyAction Property { get; set; }
    [BindProperty]
    public DiceAction Dice { get; set; }
    
    public CardActions Type { get; set; }
    public int Group { get; set; }
    public ActionConfig? Config { get; set; }
    public bool AddingConfig { get; set; }
    public bool AddingAction { get; set; }
    public Card? Card { get; set; }

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

    public bool SetupAction(int id)
    {
        switch (Type)
        {
            case CardActions.MOVE:
                if (AddingAction)
                {
                    Move = new MoveAction
                    {
                        Group = Group
                    };
                }
                else
                {
                    var move = _cardActionService.GetAction(Card!.Id, Group, id, Type);
                    if (move == null) return false;
                    Move = (MoveAction)move;
                    Move.Group = Group;
                }
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
                    //TODO
                }
                else
                {
                    
                }

                return false;
                break;
            case CardActions.CARD:
                if (AddingAction)
                {
                    //TODO
                }
                else
                {
                    
                }

                return false;
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

        res = SetupAction(id);
        return res ? Page() : RedirectToPage(nameof(Index), new {id = card});
    }

    public async Task<IActionResult> OnPost(int card, int id, int group, int action)
    {
        var res = await Setup(card, id, group, action);
        if(!res) RedirectToPage(nameof(Index), new {id = card});

        ICardActionModel? model = null;
        switch (Type)
        {
            case CardActions.MOVE:
                model = Move;

                foreach (var key in ModelState.Keys.Where(key => !key.Contains("Move")))
                {
                    ModelState.Remove(key);
                }
                
                if (!Move.IsAdvance && Move.Value <= 0)
                {
                    ModelState.AddModelError("Move.Value", "You must enter a value for the number of spaces the player moves.");
                }
                
                break;
            case CardActions.PAY:
                model = Pay;
                
                foreach (var key in ModelState.Keys.Where(key => !key.Contains("Pay")))
                {
                    ModelState.Remove(key);
                }

                if (Pay.PayTo == PayTo.PLAYER && Pay.PlayerAction == PlayerAction.NONE)
                {
                    ModelState.AddModelError("Pay.PlayerAction", "You must select a player action when paying to/receiving from a player.");
                }
                
                break;
            case CardActions.PROPERTY:
                model = Property;
                
                foreach (var key in ModelState.Keys.Where(key => !key.Contains("Property")))
                {
                    ModelState.Remove(key);
                }
                
                break;
            case CardActions.DICE:
                model = Dice;
                
                foreach (var key in ModelState.Keys.Where(key => !key.Contains("Dice")))
                {
                    ModelState.Remove(key);
                }
                
                break;
            case CardActions.RESET:
                break;
            case CardActions.CARD:
                break;
            default:
                return Page();
        }
        
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