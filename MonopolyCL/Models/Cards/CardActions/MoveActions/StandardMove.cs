using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MonopolyCL.Models.Cards.CardActions.MoveActions;

public class StandardMove : MoveBaseEvent
{
    [Range(0, 39)]
    public int Value { get; set; }  //Advance => board index, !Advance => spaces.
    
    [DisplayName("Advance to Space on Board?")]
    public bool IsAdvance { get; set; }
    
    public override void Validate(ModelStateDictionary modelState)
    {
        MoveType = MoveType.STANDARD;
        
        if (!IsAdvance && Value <= 0)
        {
            modelState.AddModelError("Move.Value", "You must enter a value for the number of spaces the player moves.");
        }
        
        base.Validate(modelState);
    }
}