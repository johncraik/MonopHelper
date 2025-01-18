using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopolyCL.Models.Game;

namespace MonopolyCL.Data;

public class GameContext
{
    public GameDbSet<GameDM> Games { get; init; }
    public GameDbSet<TurnOrder> TurnOrders { get; init; }
    
    public GameContext(GameDbContext context, UserInfo userInfo)
    {
        Games = new GameDbSet<GameDM>(context, userInfo);
        TurnOrders = new GameDbSet<TurnOrder>(context, userInfo);
    }
}