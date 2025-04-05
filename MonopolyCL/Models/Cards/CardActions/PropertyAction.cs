using System.ComponentModel;

namespace MonopolyCL.Models.Cards.CardActions;

public class PropertyAction : ICardActionModel
{
    public int ActionId { get; set; }
    public int Group { get; set; }
    public CardActions Type { get; } = CardActions.PROPERTY;
    
    [DisplayName("Take Property/Set?")]
    public bool IsTake { get; set; } //Handing property back (false) or getting property (true)
    public PropertyFrom Source { get; set; }
    [DisplayName("Property Set?")]
    public bool IsSet { get; set; }
    [DisplayName("Swap Property/Set?")]
    public bool IsSwap { get; set; }
}

public enum PropertyFrom
{
    BANK,
    FREE_PARKING,
    PLAYER
}