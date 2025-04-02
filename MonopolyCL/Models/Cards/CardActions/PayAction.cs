namespace MonopolyCL.Models.Cards.CardActions;

public class PayAction
{
    public bool IsPay { get; set; } = true;
    public int Value { get; set; } //Pay/Receive amount
    public PayTo PayTo { get; set; }
    public PayPlayer PayPlayer { get; set; }
    public PayMultiplier Multiplier { get; set; }
}

public enum PayTo
{
    BANK,
    FREE_PARKING,
    PLAYER
}

public enum PayPlayer
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