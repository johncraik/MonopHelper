using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MonopolyCL.Models.Cards.CardActions.EventActions;

public class JailEvent : EventBaseAction
{
    [DisplayName("Jail Event Type")]
    public JailEventType JailEventType { get; set; }
    
    [DisplayName("Turns to Remain in Jail")]
    public int Turns { get; set; }

    [DisplayName("Swap with Current Player?")]
    public bool IsSwapCurrentPlayer { get; set; } = true;

    public override void Validate(ModelStateDictionary modelState)
    {
        EventType = EventType.JAIL;

        switch (JailEventType)
        {
            case JailEventType.STAY_IN_JAIL:
                if (Turns <= 0)
                {
                    modelState.AddModelError("Event.Turns", "You must specify the number of turns the player must stay in jail for.");
                }
                else if (PlayerAction == PlayerAction.ALL)
                {
                    modelState.AddModelError("Event.PlayerAction", "You cannot send everyone to jail for a certain number of turns.");
                }

                IsSwapCurrentPlayer = false;
                break;
            case JailEventType.SWAP:
                if (PlayerAction != PlayerAction.SPECIFIC_PLAYER)
                {
                    modelState.AddModelError("Event.PlayerAction", "You can only swap with a specific player.");
                }
                
                Turns = 0;
                break;
            default:
                Turns = 0;
                IsSwapCurrentPlayer = false;
                break;
        }
        
        base.Validate(modelState);
    }
}

public enum JailEventType
{
    STAY_IN_JAIL,
    SWAP,
    RECEIVE_RENT
}