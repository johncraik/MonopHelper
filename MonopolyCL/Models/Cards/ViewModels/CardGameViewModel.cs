using MonopolyCL.Models.Cards;

namespace MonopolyCL.Models.Cards.ViewModels;

public class CardGameViewModel
{
    public CardGame Game { get; set; }
    public List<(Card Card, uint Index)> Cards { get; set; }
    public List<(CardType Type, uint CurrentIndex)> Types { get; set; }
}