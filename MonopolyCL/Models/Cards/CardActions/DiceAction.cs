namespace MonopolyCL.Models.Cards.CardActions;

public class DiceAction : ICardActionModel
{
    public int Group { get; set; }
    public CardActions Type { get; set; } = CardActions.DICE;
    
    public DiceConvert Convert { get; set; }
    public PlayerAction PlayerAction { get; set; }
}

public enum DiceConvert
{
    TO_DOUBLE,
    TO_TRIPLE
}