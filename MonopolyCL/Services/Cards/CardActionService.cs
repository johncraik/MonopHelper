using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MonopHelper.Authentication;
using MonopolyCL.Data;
using MonopolyCL.Dictionaries;
using MonopolyCL.Models.Cards.CardActions;
using Newtonsoft.Json;

namespace MonopolyCL.Services.Cards;

public class CardActionService
{
    private readonly UserInfo _userInfo;
    private readonly CardContext _cardContext;
    private readonly FilePathProvider _filePathProvider;

    public CardActionService(UserInfo userInfo, 
        CardContext cardContext,
        IConfiguration config)
    {
        _userInfo = userInfo;
        _cardContext = cardContext;
        _filePathProvider = new FilePathProvider(config);
    }

    private const string ActionPath = "/CardActions";

    public async Task<List<ICardActionModel>?> GetCardActions(int cardId)
    {
        var actionConfig = await _cardContext.CardActionSet.Query().Include(ac => ac.Card)
            .FirstOrDefaultAsync(ac => ac.CardId == cardId);
        if (actionConfig == null) return [];

        if (actionConfig.Card.TenantId != _userInfo.TenantId 
            && actionConfig.Card.TenantId != DefaultsDictionary.MonopolyTenant) return null;

        var actionList = new List<ICardActionModel>();
        
        var groups = actionConfig.GetGroups();
        var groupId = 1;
        var actionId = 1;
        foreach (var group in groups)
        {
            var actions = group.Split(ActionConfig.AndOperator, StringSplitOptions.RemoveEmptyEntries);
            foreach (var a in actions)
            {
                var success = int.TryParse(a, out var type);
                if(!success) continue;
                
                var action = GetAction(cardId, actionId, (CardActions)type);
                if(action == null) continue;
                
                action.Group = groupId;
                actionList.Add(action);
                actionId++;
            }

            groupId++;
        }

        return actionList;
    }

    private ICardActionModel? GetAction(int cardId, int actionId, CardActions type)
    {
        var path = $"{ActionPath}/{cardId}/Action{actionId}.txt";
        var file = _filePathProvider.GetFile(path);

        return type switch
        {
            CardActions.MOVE => JsonConvert.DeserializeObject<MoveAction>(file),
            CardActions.PAY => JsonConvert.DeserializeObject<PayAction>(file),
            CardActions.PROPERTY => JsonConvert.DeserializeObject<PropertyAction>(file),
            CardActions.DICE => JsonConvert.DeserializeObject<DiceAction>(file),
            CardActions.RESET => null,
            CardActions.CARD => null,
            _ => null
        };
    }
}