using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MonopolyCL.Models.Cards.CardActions.EventActions;

public class ForceMortgageEvent : EventBaseAction
{
    [DisplayName("Number of Properties")]
    [Range(1, 28)]
    public int Properties { get; set; }
    
    [DisplayName("Number of Turns")]
    [Range(1, int.MaxValue)]
    public int Turns { get; set; }
    
    [DisplayName("Receive Mortgage Value?")]
    public bool IsReceiveValue { get; set; } = true;
    
    [DisplayName("Pay Mortgage Penalty?")] 
    public bool IsPayPenalty { get; set; } = true;

    public override void Validate(ModelStateDictionary modelState)
    {
        EventType = EventType.FORCE_MORTGAGE;
        EndCondition = EventEndCondition.TURNS;
        base.Validate(modelState);
    }
}