using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopolyCL.Models.Cards.Actions;

namespace MonopolyCL.Data;

public class CardActionContext
{
    public GameDbSet<CardAction> CardActions { get; init; }
    public GameDbSet<AdvanceAction> AdvanceActions { get; init; }
    public GameDbSet<MoveAction> MoveActions { get; init; }
    public GameDbSet<KeepAction> KeepActions { get; init; }
    public GameDbSet<ChoiceAction> ChoiceActions { get; init; }
    public GameDbSet<PayPlayerAction> PayPlayerActions { get; init; }
    public GameDbSet<StreetRepairsAction> StreetRepairsActions { get; init; }
    
    public CardActionContext(GameDbContext context, UserInfo userInfo)
    {
        CardActions = new GameDbSet<CardAction>(context, userInfo);
        AdvanceActions = new GameDbSet<AdvanceAction>(context, userInfo);
        MoveActions = new GameDbSet<MoveAction>(context, userInfo);
        KeepActions = new GameDbSet<KeepAction>(context, userInfo);
        ChoiceActions = new GameDbSet<ChoiceAction>(context, userInfo);
        PayPlayerActions = new GameDbSet<PayPlayerAction>(context, userInfo);
        StreetRepairsActions = new GameDbSet<StreetRepairsAction>(context, userInfo);
    }
}