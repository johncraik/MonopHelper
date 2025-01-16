using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopolyCL.Data.Migrations;
using MonopolyCL.Models.Cards.Game;
using MonopolyCL.Models.Game;

namespace MonopolyCL.Data;

public class GameContext
{
    public GameDbSet<GameDM> Games { get; init; }
    public GameDbSet<TurnOrder> TurnOrders { get; init; }
    public GameDbSet<GameCard> GameCards { get; init; }
    public GameDbSet<GameType> GameCardTypes { get; init; }
    
    public GameContext(GameDbContext context, UserInfo userInfo)
    {
        Games = new GameDbSet<GameDM>(context, userInfo);
        TurnOrders = new GameDbSet<TurnOrder>(context, userInfo);
        GameCards = new GameDbSet<GameCard>(context, userInfo);
        GameCardTypes = new GameDbSet<GameType>(context, userInfo);
    }
}