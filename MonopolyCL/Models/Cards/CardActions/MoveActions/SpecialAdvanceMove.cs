using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MonopolyCL.Models.Cards.CardActions.MoveActions;

public class SpecialAdvanceMove : MoveBaseAction
{
    [DisplayName("Advance To")]
    public AdvanceTo AdvanceTo { get; set; }
    
    [DisplayName("Owner of Property")]
    public PlayerAction AdvanceOwner { get; set; }
    
    [DisplayName("Chance Space?")]
    public bool IsChance { get; set; }

    [DisplayName("Nearest?")]
    public bool IsNearest { get; set; } = true;
    
    public override void Validate(ModelStateDictionary modelState)
    {
        MoveType = MoveType.SPECIAL_ADVANCE;

        if (AdvanceTo == AdvanceTo.CARD || AdvanceTo == AdvanceTo.TAX)
        {
            AdvanceOwner = PlayerAction.CURRENT_PLAYER;
        }
        
        base.Validate(modelState);
    }
}

public enum AdvanceTo
{
    SET_PROPERTY,
    STATION,
    UTILITY,
    CARD,
    TAX
}