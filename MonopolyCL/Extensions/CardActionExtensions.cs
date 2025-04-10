using Microsoft.AspNetCore.Mvc.Rendering;
using MonopolyCL.Models.Cards.CardActions;
using MonopolyCL.Models.Cards.CardActions.EventActions;

namespace MonopolyCL.Extensions;

public static class CardActionExtensions
{
    public static List<SelectListItem> GetCardActionList()
    {
        var vals = Enum.GetValues(typeof(CardActions)).Cast<CardActions>();
        return vals.Select(val => new SelectListItem
        {
            Text = val.GetDisplayName(), 
            Value = ((int)val).ToString()
        }).OrderBy(a => a.Text).ToList();
    }
    
    public static List<SelectListItem> GetPlayConditionList()
    {
        var vals = Enum.GetValues(typeof(PlayCondition)).Cast<PlayCondition>();
        return vals.Select(val => new SelectListItem
        {
            Text = val.GetDisplayName(), 
            Value = ((int)val).ToString()
        }).ToList();
    }

    public static List<SelectListItem> GetPayToList(this ICardActionModel model)
    {
        var vals = Enum.GetValues(typeof(PayTo)).Cast<PayTo>();
        return vals.Select(val => new SelectListItem
        {
            Text = val.GetDisplayName(), 
            Value = ((int)val).ToString()
        }).ToList();
    }
    
    public static List<SelectListItem> GetPlayerActionList(this ICardActionModel model)
    {
        var vals = Enum.GetValues(typeof(PlayerAction)).Cast<PlayerAction>();
        return vals.Select(val => new SelectListItem
        {
            Text = val.GetDisplayName(), 
            Value = ((int)val).ToString()
        }).ToList();
    }
    
    public static List<SelectListItem> GetMultiplierList(this ICardActionModel model)
    {
        var vals = Enum.GetValues(typeof(PayMultiplier)).Cast<PayMultiplier>();
        return vals.Select(val => new SelectListItem
        {
            Text = val.GetDisplayName(), 
            Value = ((int)val).ToString()
        }).ToList();
    }
    
    public static List<SelectListItem> GetSourceList(this ICardActionModel model)
    {
        var vals = Enum.GetValues(typeof(PropertyFrom)).Cast<PropertyFrom>();
        return vals.Select(val => new SelectListItem
        {
            Text = val.GetDisplayName(), 
            Value = ((int)val).ToString()
        }).ToList();
    }
    
    public static List<SelectListItem> GetConvertList(this ICardActionModel model)
    {
        var vals = Enum.GetValues(typeof(DiceConvert)).Cast<DiceConvert>();
        return vals.Select(val => new SelectListItem
        {
            Text = val.GetDisplayName(), 
            Value = ((int)val).ToString()
        }).ToList();
    }
    
    public static List<SelectListItem> GetResetList(this ICardActionModel model)
    {
        var vals = Enum.GetValues(typeof(ResetType)).Cast<ResetType>();
        return vals.Select(val => new SelectListItem
        {
            Text = val.GetDisplayName(), 
            Value = ((int)val).ToString()
        }).ToList();
    }

    public static List<SelectListItem> GetEventTypeList(this ICardActionModel model)
    {
        var vals = Enum.GetValues(typeof(EventType)).Cast<EventType>();
        var list = vals.Select(val => new SelectListItem
        {
            Text = val.GetDisplayName(), 
            Value = ((int)val).ToString()
        }).ToList();
        
        list.Insert(0, new SelectListItem
        {
            Text = "Select an Event Type",
            Value = "-1"
        });
        return list;
    }
}