using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MonopolyCL.Models.Cards.CardActions.EventActions;

public class FreeParkingEvent : EventBaseAction
{
    [DisplayName("Free Parking Event Type")]
    public FreeParkingEventType FpType { get; set; }

    [DisplayName("Hand in Property?")] 
    public bool IsHandInProperty { get; set; } = true;

    public override void Validate(ModelStateDictionary modelState)
    {
        EventType = EventType.FREE_PARKING;
        EndCondition = EventEndCondition.DEFAULT;

        if (PlayerAction == PlayerAction.ALL && (FpType == FreeParkingEventType.RECEIVE_FINES || FpType == FreeParkingEventType.FROM_PLAYER))
        {
            modelState.AddModelError("Event.PlayerAction", "A free parking event for receiving fines or taking from another player cannot be all players.");
        }

        if (FpType == FreeParkingEventType.FROM_PLAYER && PlayerAction == PlayerAction.CURRENT_PLAYER)
        {
            modelState.AddModelError("Event.PlayerAction", "You cannot take the money from free parking from yourself.");
        }
        
        base.Validate(modelState);
    }
}

public enum FreeParkingEventType
{
    RECEIVE_FINES,
    FROM_PLAYER,
    NO_MONEY,
    ALL_MONEY
}