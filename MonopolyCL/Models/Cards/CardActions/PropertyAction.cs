namespace MonopolyCL.Models.Cards.CardActions;

public class PropertyAction : ICardActionModel
{
    public int Group { get; set; }
    public CardActions Type { get; } = CardActions.PROPERTY;
    
    public bool IsTake { get; set; } //Handing property back (false) or getting property (true)
    public PropertyFrom Source { get; set; }
    public bool IsSet { get; set; }
    public bool IsSwap { get; set; }
}

public enum PropertyFrom
{
    BANK,
    FREE_PARKING,
    PLAYER
}