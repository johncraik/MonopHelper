using MonopolyCL.Models.Cards.Enums;

namespace MonopolyCL.Models.Cards.Actions;

public class ActionModel
{
    public AdvanceAction? AdvanceAction { get; set; }
    public MoveAction? MoveAction { get; set; }
    public KeepAction? KeepAction { get; set; }
    public ChoiceAction? ChoiceAction { get; set; }
    public PayPlayerAction? PayPlayerAction { get; set; }
    public StreetRepairsAction? StreetRepairsAction { get; set; }

    public void SetModel(CARD_ACTION actionNum, ICardAction action)
    {
        switch (actionNum)
        {
            case CARD_ACTION.ADVANCE:
                AdvanceAction = (AdvanceAction)action;
                break;
            case CARD_ACTION.MOVE:
                MoveAction = (MoveAction)action;
                break;
            case CARD_ACTION.KEEP_CARD:
                KeepAction = (KeepAction)action;
                break;
            case CARD_ACTION.CHOICE:
                ChoiceAction = (ChoiceAction)action;
                break;
            case CARD_ACTION.PAY_PLAYER:
                PayPlayerAction = (PayPlayerAction)action;
                break;
            case CARD_ACTION.STREET_REPAIRS:
                StreetRepairsAction = (StreetRepairsAction)action;
                break;
        }
    }
    
    public void SetModel(CARD_ACTION actionNum)
    {
        switch (actionNum)
        {
            case CARD_ACTION.ADVANCE:
                AdvanceAction = new AdvanceAction();
                break;
            case CARD_ACTION.MOVE:
                MoveAction = new MoveAction();
                break;
            case CARD_ACTION.KEEP_CARD:
                KeepAction = new KeepAction();
                break;
            case CARD_ACTION.CHOICE:
                ChoiceAction = new ChoiceAction();
                break;
            case CARD_ACTION.PAY_PLAYER:
                PayPlayerAction = new PayPlayerAction();
                break;
            case CARD_ACTION.STREET_REPAIRS:
                StreetRepairsAction = new StreetRepairsAction();
                break;
        }
    }
}