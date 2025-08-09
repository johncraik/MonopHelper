using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace MonopolyCL.Models.Cards.CardActions.EventActions;

public class EventBaseAction : ICardActionModel
{
    public int ActionId { get; set; }
    public int Group { get; set; }
    public CardActions Type { get; } = CardActions.EVENT;

    [DisplayName("Event Type")] 
    public EventType EventType { get; set; } = EventType.NONE;
    
    [DisplayName("Player")]
    public PlayerAction PlayerAction { get; set; }
    
    [DisplayName("End Condition")]
    public EventEndCondition EndCondition { get; set; }

    public virtual void Validate(ModelStateDictionary modelState)
    {
        if (EventType == EventType.NONE)
        {
            modelState.AddModelError("Event.EventType", "Please select a type of event action.");
        }
    }

    public ICardActionModel? GetEventType(string file) =>
        EventType switch
        {
            EventType.TURNS => JsonConvert.DeserializeObject<TurnsEvent>(file),
            EventType.FORCE_MORTGAGE => JsonConvert.DeserializeObject<ForceMortgageEvent>(file),
            EventType.UNIQUE_SPACE => JsonConvert.DeserializeObject<SpaceEvent>(file),
            EventType.GO => JsonConvert.DeserializeObject<GoEvent>(file),
            EventType.FREE_PARKING => JsonConvert.DeserializeObject<FreeParkingEvent>(file),
            EventType.JAIL => JsonConvert.DeserializeObject<JailEvent>(file),
            _ => null
        };
}

public enum EventType
{
    NONE = -1,
    TURNS,
    FORCE_MORTGAGE,
    UNIQUE_SPACE,
    GO,
    FREE_PARKING,
    JAIL
}

public enum EventEndCondition
{
    DEFAULT,
    TURNS,
    DOUBLE,
    TRIPLE,
    IN_JAIL
}