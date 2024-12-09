using MonopolyCL.Models.Cards.Actions;

namespace MonopolyCL.Models.Cards.Game;

public class GameCardViewModel
{
    public Card Card { get; set; }
    public ICardAction? Action { get; set; }
    public bool HasAction => Action != null;
}