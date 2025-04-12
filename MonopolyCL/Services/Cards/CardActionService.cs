using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MonopHelper.Authentication;
using MonopolyCL.Data;
using MonopolyCL.Dictionaries;
using MonopolyCL.Models.Cards.CardActions;
using MonopolyCL.Models.Cards.CardActions.EventActions;
using MonopolyCL.Models.Cards.CardActions.MoveActions;
using Newtonsoft.Json;

namespace MonopolyCL.Services.Cards;

public class CardActionService
{
    private readonly UserInfo _userInfo;
    private readonly CardContext _cardContext;
    private readonly ILogger<CardActionService> _logger;
    private readonly FilePathProvider _filePathProvider;

    public CardActionService(UserInfo userInfo, 
        CardContext cardContext,
        IConfiguration config,
        ILogger<CardActionService> logger)
    {
        _userInfo = userInfo;
        _cardContext = cardContext;
        _logger = logger;
        _filePathProvider = new FilePathProvider(config);
    }

    private const string ActionPath = "/CardActions";

    public async Task<(ActionConfig? Config, bool Invalid)> GetActionConfig(int cardId)
    {
        var actionConfig = await _cardContext.CardActionSet.Query()
            .Include(ac => ac.Card)
            .ThenInclude(c => c.CardType)
            .FirstOrDefaultAsync(ac => ac.CardId == cardId);
        if (actionConfig == null) return (null, false);

        if (actionConfig.Card.TenantId != _userInfo.TenantId 
            && actionConfig.Card.TenantId != DefaultsDictionary.MonopolyTenant) return (null, true);

        return (actionConfig, false);
    }
    
    public async Task<(ActionConfig? config, List<ICardActionModel>? Actions)> GetCardActions(int cardId)
    {
        var (actionConfig, _) = await GetActionConfig(cardId);
        if (actionConfig == null) return (null, []);

        var actionList = new List<ICardActionModel>();
        
        var groups = actionConfig.GetGroups();
        var groupId = 1;
        foreach (var group in groups)
        {
            var actionId = 1;
            var actions = group.Split(ActionConfig.AndOperator, StringSplitOptions.RemoveEmptyEntries);
            foreach (var a in actions)
            {
                var success = int.TryParse(a, out var type);
                if(!success) continue;
                
                var action = GetAction(cardId, groupId, actionId, (CardActions)type);
                if(action == null) continue;
                
                action.Group = groupId;
                actionList.Add(action);
                actionId++;
            }

            groupId++;
        }
        
        var types = actionList.Select(a => a.Type).Distinct().Sum(a => (int)a);
        actionConfig.Actions = (CardActions)types;
        await _cardContext.CardActionSet.UpdateAsync(actionConfig);

        return (actionConfig, actionList);
    }

    public ICardActionModel? GetAction(int cardId, int groupId, int actionId, CardActions type)
    {
        try
        {
            var path = $"{ActionPath}/{cardId}/Action{groupId}-{actionId}.txt";
            var file = _filePathProvider.GetFile(path);

            return type switch
            {
                CardActions.MOVE => JsonConvert.DeserializeObject<MoveBaseAction>(file)?.GetMoveType(file),
                CardActions.PAY => JsonConvert.DeserializeObject<PayAction>(file),
                CardActions.PROPERTY => JsonConvert.DeserializeObject<PropertyAction>(file),
                CardActions.DICE => JsonConvert.DeserializeObject<DiceAction>(file),
                CardActions.RESET => JsonConvert.DeserializeObject<ResetAction>(file),
                CardActions.TAKE_CARD => JsonConvert.DeserializeObject<TakeCardAction>(file),
                CardActions.EVENT => JsonConvert.DeserializeObject<EventBaseAction>(file)?.GetEventType(file),
                _ => null
            };
        }
        catch(Exception ex)
        {
            _logger.LogError($"Cant get card action! -- {ex}");
            return null;
        }
    }

    public async Task<bool> TryAddConfig(ActionConfig config)
    {
        if (config.CardId <= 0) return false;

        await _cardContext.CardActionSet.AddAsync(config);
        return true;
    }

    public async Task<bool> TryUpdateConfig(bool isKeep, int selectedCondition, ActionConfig config, List<ICardActionModel> actions)
    {
        if (config.CardId <= 0) return false;
        if (!Enum.IsDefined(typeof(PlayCondition), selectedCondition)) return false;

        var types = actions.Select(a => a.Type).Distinct().Sum(a => (int)a);
        config.Actions = (CardActions)types;
        config.IsKeep = isKeep;
        config.PlayCondition = isKeep switch
        {
            true => (PlayCondition)selectedCondition,
            _ => PlayCondition.NONE
        };

        await _cardContext.CardActionSet.UpdateAsync(config);
        return true;
    }

    private int GetNextActionId(int cardId, int group)
    {
        var basePath = $"{ActionPath}/{cardId}";
        var files = _filePathProvider.GetFileNames(basePath);
        var lastVal = 0;
        foreach (var file in files)
        {
            var actionValues = (file[(file.IndexOf('n')+1)..]).Split('-');
            if(actionValues[0] != group.ToString()) continue;

            var id = actionValues[1];
            var success = int.TryParse(id[..id.IndexOf('.')], out var val);
            if(!success) continue;

            lastVal = val;
        }

        return lastVal + 1;
    }

    public async Task<int> UpdateActionConfig(ActionConfig? config, int cardId, int group, int type)
    {
        if (config == null)
        {
            config = new ActionConfig
            {
                CardId = cardId
            };
            await _cardContext.CardActionSet.AddAsync(config);
        }
        
        var groupId = config.AddToGroup(group, type);
        await _cardContext.CardActionSet.UpdateAsync(config);
        return groupId;
    }
    
    public async Task TryCreateAction(ActionConfig? config, ICardActionModel? model, int cardId)
    {
        if(model == null) return;
        
        var newGroup = await UpdateActionConfig(config, cardId, model.Group, (int)model.Type);
        if (model.Group == 0) model.Group = newGroup;
        
        var actionId = GetNextActionId(cardId, model.Group);
        model.ActionId = actionId;
        
        var json = JsonConvert.SerializeObject(model);
        await _filePathProvider.SaveFile($"{ActionPath}/{cardId.ToString()}", $"Action{model.Group}-{actionId}.txt", json);
    }

    public async Task TryUpdateAction(ICardActionModel? model, int cardId)
    {
        if (model == null) return;

        var json = JsonConvert.SerializeObject(model);
        await _filePathProvider.SaveFile($"{ActionPath}/{cardId.ToString()}", $"Action{model.Group}-{model.ActionId}.txt", json);
    }

    public async Task ClearActions(ActionConfig? config, List<ICardActionModel> models, int cardId)
    {
        if (config != null)
        {
            await _cardContext.CardActionSet.RemoveAsync(config);
        }

        var path = $"{ActionPath}/{cardId}";
        _filePathProvider.DeleteFiles(path);
    }
}