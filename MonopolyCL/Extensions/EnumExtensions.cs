using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Storage;
using MonopolyCL.Models.Cards.CardActions.EventActions;

namespace MonopolyCL.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum e) =>
        CultureInfo.CurrentCulture.TextInfo.ToTitleCase(e.ToString().Replace('_', ' ').ToLower());

    public static List<SelectListItem> GetSelectList(this Enum e, bool insertSelect = false, string selectText = "Select an Item")
    {
        var vals = Enum.GetValues(e.GetType()).Cast<Enum>();
        
        var list = vals.Select(val => new SelectListItem
        {
            Text = val.GetDisplayName(), 
            Value = val.GetHashCode().ToString()
        }).ToList();

        if (!insertSelect) return list;
        
        list.Insert(0, new SelectListItem
        {
            Text = selectText,
            Value = "-1"
        });
        return list;
    }
}