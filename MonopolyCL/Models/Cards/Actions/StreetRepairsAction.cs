using System.ComponentModel;

namespace MonopolyCL.Models.Cards.Actions;

public class StreetRepairsAction : ICardAction
{
    public int Id { get; set; }
    [DisplayName("House Cost")]
    public int HouseCost { get; set; }
    [DisplayName("Hotel Cost")]
    public int HotelCost { get; set; }
}