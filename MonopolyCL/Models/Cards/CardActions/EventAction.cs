using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MonopolyCL.Models.Cards.CardActions;

public class EventAction : ICardActionModel
{
    public int ActionId { get; set; }
    public int Group { get; set; }
    public CardActions Type { get; } = CardActions.EVENT;
    
    [Range(0, int.MaxValue)]
    public int Value { get; set; }
    [DisplayName("Event Type")]
    public EventType EventType { get; set; }
    [DisplayName("End Condition")]
    public EventCondition Condition { get; set; }
    [DisplayName("Type of Free Parking Event")]
    public FreeParkingEvent FpEvent { get; set; }
    [DisplayName("Player Action")]
    public PlayerAction PlayerAction { get; set; }
    [DisplayName("Pay on GO?")]
    public bool IsPayOnGo { get; set; }
    [DisplayName("Receive Rent in Jail?")]
    public bool IsReceiveRent { get; set; }
    
    public void Validate(ModelStateDictionary modelState)
    {
        if (EventType is EventType.EXTRA_TURNS or EventType.MISS_TURNS
            && Condition != EventCondition.TURNS)
        {
            modelState.AddModelError("Event.Condition", "The selected event can only end based on turns.");
        }

        if (EventType == EventType.FREE_PARKING
            && FpEvent == FreeParkingEvent.NONE)
        {
            modelState.AddModelError("Event.FpEvent", "You must specify the type of free parking event.");
        }

        if (EventType is not (EventType.TAX or EventType.STATIONS or EventType.UTILITIES 
                or EventType.FREE_PARKING or EventType.JAIL)
            && Value <= 0)
        {
            modelState.AddModelError("Event.Value", "The value cannot be 0.");
        }

        switch (EventType)
        {
            case EventType.FREE_PARKING:
                Value = 0;
                Condition = EventCondition.DEFAULT;
                break;
            case EventType.GO:
                Condition = EventCondition.DEFAULT;
                break;
        }
    }
}

public enum EventType
{
    EXTRA_TURNS,
    MISS_TURNS,
    MORTGAGE,
    JAIL,
    TAX,
    STATIONS,
    UTILITIES,
    FREE_PARKING,
    GO
}

public enum FreeParkingEvent
{
    NONE = 0,
    RECEIVE_FINES,
    FROM_PLAYER,
    NO_MONEY,
    ALL_MONEY
}

public enum EventCondition
{
    DEFAULT = 0,
    TURNS,
    DOUBLE,
    TRIPLE,
    JAIL
}