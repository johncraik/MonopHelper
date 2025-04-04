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
        string.IsNullOrEmpty(Groups) ? [] : Groups.Split(OrOperator, StringSplitOptions.RemoveEmptyEntries);
    
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
    CARD = 32
}

public interface ICardActionModel
{
    public int Group { get; set; }
    public CardActions Type { get; }
}