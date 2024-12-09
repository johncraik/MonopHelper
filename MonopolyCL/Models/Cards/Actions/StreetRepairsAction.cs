namespace MonopolyCL.Models.Cards.Actions;

public class StreetRepairsAction : ICardAction
{
    public int Id { get; set; }
    public int HouseCost { get; set; }
    public int HotelCost { get; set; }
}