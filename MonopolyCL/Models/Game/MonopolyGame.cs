using MonopolyCL.Models.Boards;
using MonopolyCL.Models.Cards;
using MonopolyCL.Models.Players;

namespace MonopolyCL.Models.Game;

public class MonopolyGame
{
    public Board Board { get; set; }
    public List<IPlayer> Players { get; set; }
    public List<Card> Cards { get; set; }
    public GAME_RULES Rules { get; set; }
}