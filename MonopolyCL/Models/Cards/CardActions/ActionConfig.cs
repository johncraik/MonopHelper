using System.ComponentModel.DataAnnotations.Schema;

namespace MonopolyCL.Models.Cards.CardActions;

public class ActionConfig
{
    public int Id { get; set; }
    public bool IsKeep { get; set; }
    public CardActions Actions { get; set; }
    public string? Groups { get; set; }
    
    public int CardId { get; set; }
    [ForeignKey(nameof(CardId))]
    public virtual Card Card { get; set; }
    

    public string[] GetGroups() => 
        string.IsNullOrEmpty(Groups) ? [] : Groups.Split(GroupDelimiter, StringSplitOptions.RemoveEmptyEntries);

    public const string GroupDelimiter = "-";
    public const string OrOperator = "|";
    public const string AndOperator = "&";
}

[Flags]
public enum CardActions
{
    MOVE = 1,
    PAY = 2,
    PROPERTY = 4,
    DICE = 8,
    RESET = 16,
    CARD = 32
}

/*
 * Example:
 * Return a set -OR- Pay £5,000 AND £300
 * Property -OR- Pay AND Advance => 8 -OR- 4 AND 4 => 12
 * 
 * 8|,4&4
 */