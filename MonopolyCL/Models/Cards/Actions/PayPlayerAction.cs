using MonopolyCL.Models.Cards.Enums;

namespace MonopolyCL.Models.Cards.Actions;

public class PayPlayerAction : ICardAction
{
    public int Id { get; set; }
    public PAY_PLAYER PayToType { get; set; }
    public int AmountToPlayer { get; set; }
}