using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopolyCL.Models.Players.DataModel;

namespace MonopolyCL.Data;

public class PlayerContext
{
    public GameDbSet<PlayerDM> Players { get; init; }
    public GameDbSet<GamePlayer> GamePlayers { get; init; }
    public GameDbSet<DiceNumbers> DiceNums { get; init; }
    public GameDbSet<PlayerToCard> PlayersToCards { get; init; }
    public GameDbSet<PlayerToProperty> PlayersToProperties { get; init; }
    
    public PlayerContext(GameDbContext context, UserInfo userInfo)
    {
        Players = new GameDbSet<PlayerDM>(context, userInfo);
        GamePlayers = new GameDbSet<GamePlayer>(context, userInfo);
        DiceNums = new GameDbSet<DiceNumbers>(context, userInfo);
        PlayersToCards = new GameDbSet<PlayerToCard>(context, userInfo);
        PlayersToProperties = new GameDbSet<PlayerToProperty>(context, userInfo);
    }
}