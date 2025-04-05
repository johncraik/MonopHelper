using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonopolyCL.Extensions;

namespace MonopolyCL.Models.Cards.CardActions;

public class ActionConfig
{
    public int Id { get; set; }
    [DisplayName("Player can Keep Card")]
    public bool IsKeep { get; set; }
    public CardActions Actions { get; set; }
    public string? Groups { get; set; }
    
    public int CardId { get; set; }
    [ForeignKey(nameof(CardId))]
    public virtual Card Card { get; set; }
    

    public string[] GetGroups() => 
        string.IsNullOrEmpty(Groups) ? [] : Groups.Split(OrOperator, StringSplitOptions.RemoveEmptyEntries);

    public int AddToGroup(int group, int type)
    {
        var groups = GetGroups();

        if (groups.Length == 0)
        {
            Groups = $"{type}{OrOperator}";
        }
        else if (group == 0)
        {
            Groups += type.ToString();
        }
        else
        {
            groups[group - 1] = $"{groups[group - 1]}&{type}";
            Groups = groups.Aggregate("", (current, grp) => current + $"{grp}{OrOperator}");
        }

        return groups.Length + 1;
    }
    
    public const string OrOperator = "|";   //Splits groups
    public const string AndOperator = "&";  //Splits actions in groups
}

[Flags]
public enum CardActions
{
    MOVE = 1,
    PAY = 2,
    PROPERTY = 4,
    DICE = 8,
    RESET = 16,
    CARD = 32,
    Event = 64
}

public interface ICardActionModel
{
    public int ActionId { get; set; }
    public int Group { get; set; }
    public CardActions Type { get; }
}