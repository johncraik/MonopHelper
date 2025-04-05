using Microsoft.AspNetCore.Mvc.Rendering;
using MonopolyCL.Models.Cards.CardActions;

namespace MonopolyCL.Extensions;

public static class CardActionExtensions
{
    public static List<SelectListItem> GetSelectList()
    {
        var list = new List<SelectListItem>
        {
            new()
            {
                Text = "Move Action",
                Value = "1"
            },
            new()
            {
                Text = "Pay Action",
                Value = "2"
            },
            new()
            {
                Text = "Property Action",
                Value = "4"
            },
            new()
            {
                Text = "Dice Action",
                Value = "8"
            },
            new()
            {
                Text = "Reset Action",
                Value = "16"
            },
            new()
            {
                Text = "Card Action",
                Value = "32"
            },
            new()
            {
                Text = "Event Action",
                Value = "64"
            }
        };
        return list.OrderBy(i => i.Text).ToList();
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
}