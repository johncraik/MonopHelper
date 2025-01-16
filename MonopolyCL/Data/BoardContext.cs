using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopolyCL.Models.Boards.DataModel;
using MonopolyCL.Models.Properties.DataModel;

namespace MonopolyCL.Data;

public class BoardContext
{
    public GameDbSet<BoardDM> Boards { get; init; }
    public GameDbSet<BoardToProperty> BoardsToProperties { get; init; }
    public GameDbSet<PropertyDM> Properties { get; init; }
    public GameDbSet<GameProperty> GameProperties { get; init; }
    
    public BoardContext(GameDbContext context, UserInfo userInfo)
    {
        Boards = new GameDbSet<BoardDM>(context, userInfo);
        BoardsToProperties = new GameDbSet<BoardToProperty>(context, userInfo);
        Properties = new GameDbSet<PropertyDM>(context, userInfo);
        GameProperties = new GameDbSet<GameProperty>(context, userInfo);
    }
}