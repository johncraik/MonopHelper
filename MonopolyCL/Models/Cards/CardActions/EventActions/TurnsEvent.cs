using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MonopolyCL.Models.Cards.CardActions.EventActions;

public class TurnsEvent : EventBaseAction
{
    [DisplayName("Number of Turns")]
    [Range(1, int.MaxValue)]
    public int Turns { get; set; }
    
    [DisplayName("Miss Turns?")]
    public bool IsMiss { get; set; }

    public override void Validate(ModelStateDictionary modelState)
    {
        EventType = EventType.TURNS;
        EndCondition = EventEndCondition.TURNS;
        base.Validate(modelState);
    }
}