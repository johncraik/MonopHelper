using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonopolyCL.Extensions;
using MonopolyCL.Models.Cards.CardActions;
using MonopolyCL.Services.Cards;


namespace MonopHelper.Pages.Cards.Actions;

public class Index : PageModel
{
    private readonly CardActionService _cardActionService;
    private readonly CardService _cardService;

    public Index(CardActionService cardActionService,
        CardService cardService)
    {
        _cardActionService = cardActionService;
        _cardService = cardService;
    }
    
    [BindProperty]
    public List<IGrouping<int, ICardActionModel>> Actions { get; set; } = [];
    public ActionConfig ActionConfig { get; set; }
    [BindProperty]
    [DisplayName("Player can Keep Card")]
    public bool IsKeep { get; set; }
    [BindProperty]
    [DisplayName("Play Condition")]
    public int SelectedCondition { get; set; }
    public List<SelectListItem> PlayConditions { get; set; }
    public bool Adding { get; set; }

    public class AddActionModel
    {
        public List<SelectListItem> Actions { get; set; }
        [DisplayName("Card Action")]
        public CardActions SelectedAction { get; set; }
        public int Group { get; set; }
    }
    
    [BindProperty]
    public AddActionModel AddAction { get; set; }

    public async Task Setup(int id, bool setupAddAction = true)
    {
        var (config, actions) = await _cardActionService.GetCardActions(id);
        if (actions != null)
        {
            Actions = actions.GroupBy(ca => ca.Group).ToList();
        }

        if (config != null)
        {
            ActionConfig = config;
            SelectedCondition = (int)config.PlayCondition;
        }
        else
        {
            ActionConfig = new ActionConfig
            {
                CardId = id,
                Card = (await _cardService.FindCard(id))!
            };
            Adding = true;
        }

        PlayConditions = CardActionExtensions.GetPlayConditionList();
        
        //Setup add action:
        if (setupAddAction)
        {
            AddAction = new AddActionModel
            {
                Group = 0,
                SelectedAction = 0,
                Actions = CardActionExtensions.GetCardActionList()
            };
        }
        else
        {
            AddAction.Actions = CardActionExtensions.GetCardActionList();
        }
    }
    
    public async Task<IActionResult> OnGet(int id)
    {
        await Setup(id);
        return Page();
    }

    public async Task<IActionResult> OnPostSave(int id)
    {
        await Setup(id);
        
        bool res;
        if (Adding)
        {
            ActionConfig.IsKeep = IsKeep;
            ActionConfig.PlayCondition = IsKeep switch
            {
                true => (PlayCondition)SelectedCondition,
                _ => PlayCondition.NONE
            };
            res = await _cardActionService.TryAddConfig(ActionConfig);
        }
        else
        {
            res = await _cardActionService
                .TryUpdateConfig(IsKeep, SelectedCondition, ActionConfig, Actions.SelectMany(ac =>
                    ac.Select(a => a)).ToList());
        }

        return res ? RedirectToPage(nameof(Index)) : Page();
    }

    public async Task<IActionResult> OnPostClear(int id)
    {
        await Setup(id);
        await _cardActionService.ClearActions(ActionConfig, Actions.SelectMany(ac =>
            ac.Select(a => a)).ToList(), id);

        return RedirectToPage(nameof(Index));
    }

    public async Task<IActionResult> OnPostAddAction(int id)
    {
        await Setup(id, false);
        return RedirectToPage(nameof(Edit), new {card = id, id = 0, group = AddAction.Group, action = (int)AddAction.SelectedAction});
    }
}