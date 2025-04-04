namespace MonopolyCL.Models.Cards.CardActions;

public class PayAction : ICardActionModel
{
    public int Group { get; set; }
    public CardActions Type { get; } = CardActions.PAY;
    
    public bool IsPay { get; set; } = true;
    public int Value { get; set; } //Pay/Receive amount
    public PayTo PayTo { get; set; }
    public PlayerAction PlayerAction { get; set; }
    public PayMultiplier Multiplier { get; set; }
}

public enum PayTo
{
    BANK,
    FREE_PARKING,
    PLAYER
}

public enum PlayerAction
{
    NONE = 0,
    LEFT,
    RIGHT,
    ALL
}

public enum PayMultiplier
{
    FIXED = 0,
    HOUSE,
    HOTEL,
    PROPERTY,
    SINGLE_DICE,
    DUEL_DICE,
    TRIPLE_DICE
}