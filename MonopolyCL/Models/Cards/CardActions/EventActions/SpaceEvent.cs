using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MonopolyCL.Models.Cards.CardActions.EventActions;

public class SpaceEvent : EventBaseAction
{
    [DisplayName("Board Space")]
    public BoardSpaceEvent BoardSpace { get; set; }
    
    [DisplayName("Board Space Event Type")]
    public SpaceEventType SpaceType { get; set; }
    
    [DisplayName("Multiplier Value")]
    public double MultiplierValue { get; set; }
    
    [DisplayName("Type of Multiplier")]
    public PayMultiplier MultiplierType { get; set; }

    public override void Validate(ModelStateDictionary modelState)
    {
        EventType = EventType.UNIQUE_SPACE;
        
        if (SpaceType != SpaceEventType.RENT_MULTIPLIER)
        {
            MultiplierType = PayMultiplier.FIXED;
            MultiplierValue = 0d;
        }
        else if (MultiplierType != PayMultiplier.FIXED)
        {
            MultiplierValue = 0d;
        }

        if (EndCondition == EventEndCondition.DEFAULT)
        {
            modelState.AddModelError("Event.EndCondition", "Please give a specific end condition for this action.");
        }
        
        base.Validate(modelState);
    }
}

public enum BoardSpaceEvent
{
    TAX,
    STATIONS,
    UTILITIES,
    PROPERTIES
}

public enum SpaceEventType
{
    NO_RENT,
    PAYED_TO_PLAYER,
    DOUBLE_RENT,
    RENT_MULTIPLIER
}