using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace MonopolyCL.Models.Cards.CardActions.MoveActions;

public class MoveBaseAction : ICardActionModel
{
    public int ActionId { get; set; }
    public int Group { get; set; }
    public CardActions Type { get; } = CardActions.MOVE;

    [DisplayName("Move Type")] 
    public MoveType MoveType { get; set; } = MoveType.NONE;
    
    [DisplayName("Moving Forwards?")]
    public bool IsForward { get; set; } = true;
    
    [DisplayName("Player")]
    public PlayerAction PlayerAction { get; set; }

    public virtual void Validate(ModelStateDictionary modelState)
    {
        if (MoveType == MoveType.NONE)
        {
            modelState.AddModelError("Move.MoveType", "Please select a type of move action.");
        }
    }

    public ICardActionModel? GetMoveType(string file) =>
        MoveType switch
        {
            MoveType.STANDARD => JsonConvert.DeserializeObject<StandardMove>(file),
            MoveType.SPECIAL_ADVANCE => JsonConvert.DeserializeObject<SpecialAdvanceMove>(file),
            _ => null
        };
}

public enum MoveType
{
    NONE = -1,
    STANDARD,
    SPECIAL_ADVANCE
}