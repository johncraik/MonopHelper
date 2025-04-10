using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MonopolyCL.Models.Cards.CardActions.EventActions;

public class GoEvent : EventBaseAction
{
    [Range(1, int.MaxValue)]
    public int Value { get; set; }
    
    [DisplayName("Pay when on GO?")]
    public bool IsPay { get; set; }
    
    //NOTE: Player Action means who gets value, is pay then from current player, is receive then from bank.
    
    public override void Validate(ModelStateDictionary modelState)
    {
        EventType = EventType.GO;
        EndCondition = EventEndCondition.DEFAULT;
        base.Validate(modelState);
    }
}