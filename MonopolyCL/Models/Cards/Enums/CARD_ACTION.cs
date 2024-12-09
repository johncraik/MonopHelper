namespace MonopolyCL.Models.Cards.Enums;

public enum CARD_ACTION
{
    ADVANCE,
    STREET_REPAIRS,
    PAY_PLAYER,
    KEEP_CARD,
    MOVE,
    CHOICE
}

public static class CardActionExtensions
{
    public static string GetActionColour(this CARD_ACTION a)
    {
        return a switch
        {
            CARD_ACTION.ADVANCE => "bg-adv",
            CARD_ACTION.MOVE => "bg-move",
            CARD_ACTION.KEEP_CARD => "bg-keep",
            CARD_ACTION.CHOICE => "bg-choice",
            CARD_ACTION.PAY_PLAYER => "bg-payply",
            CARD_ACTION.STREET_REPAIRS => "bg-sr",
            _ => "bg-danger text-white"
        };
    }
}