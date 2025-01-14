using MonopolyCL.Models.Boards;
using MonopolyCL.Models.Cards.ViewModels;
using MonopolyCL.Models.Players;

namespace MonopolyCL.Models.Game;

public class MonopolyGame
{
    public GameDM Game { get; set; }
    public Board Board { get; set; }
    public List<IPlayer> Players { get; set; }
    public CardGameViewModel? Cards { get; set; }
}