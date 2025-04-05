using System.ComponentModel;

namespace MonopolyCL.Models.Cards.CardActions;

public class DiceAction : ICardActionModel
{
    public int ActionId { get; set; }
    public int Group { get; set; }
    public CardActions Type { get; } = CardActions.DICE;
    
    public DiceConvert Convert { get; set; }
    [DisplayName("Player Action")]
    public PlayerAction PlayerAction { get; set; }
}

public enum DiceConvert
{
    TO_DOUBLE,
    TO_TRIPLE
}