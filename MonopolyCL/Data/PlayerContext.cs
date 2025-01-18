using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopolyCL.Models.Players.DataModel;

namespace MonopolyCL.Data;

public class PlayerContext
{
    public GameDbSet<PlayerDM> Players { get; init; }
    public GameDbSet<GamePlayer> GamePlayers { get; init; }
    public GameDbSet<DiceNumbers> DiceNums { get; set; }
    
    public PlayerContext(GameDbContext context, UserInfo userInfo)
    {
        Players = new GameDbSet<PlayerDM>(context, userInfo);
        GamePlayers = new GameDbSet<GamePlayer>(context, userInfo);
        DiceNums = new GameDbSet<DiceNumbers>(context, userInfo);
    }
}