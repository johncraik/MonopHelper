using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MonopolyCL.Models.Cards.CardActions;

public class ResetAction : ICardActionModel
{
    public int ActionId { get; set; }
    public int Group { get; set; }
    public CardActions Type { get; } = CardActions.RESET;
    
    [DisplayName("Reset Type")]
    public ResetType Reset { get; set; }
    [DisplayName("Number of Properties/Sets to Purge")]
    public int Value { get; set; }
    [DisplayName("Purge Set?")]
    public bool IsPurgeSet { get; set; }
    [DisplayName("Player Action")]
    public PlayerAction PlayerAction { get; set; }
    
    public void Validate(ModelStateDictionary modelState)
    {
        if (Reset == ResetType.PURGE && Value is <= 0 or > 28)
        {
            modelState.AddModelError("Reset.Value", "You must specify the number of properties or sets when purging.");
        }

        if (Reset == ResetType.PURGE) return;
        
        Value = 0;
        IsPurgeSet = false;
    }
}

public enum ResetType
{
    JAIL_COST,
    PURGE,
    TRIPLE_BONUS,
    FREE_PARKING_HAND_INS
}