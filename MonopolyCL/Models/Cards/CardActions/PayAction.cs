using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MonopolyCL.Models.Cards.CardActions;

public class PayAction : ICardActionModel
{
    public int ActionId { get; set; }
    public int Group { get; set; }
    public CardActions Type { get; } = CardActions.PAY;
    
    [DisplayName("Player Pays?")]
    public bool IsPay { get; set; } = true;
    [Range(1, int.MaxValue)]
    public int Value { get; set; } //Pay/Receive amount
    public PayTo PayTo { get; set; }
    [DisplayName("Player Action")]
    public PlayerAction PlayerAction { get; set; }
    [DisplayName("Multiplier")]
    public PayMultiplier Multiplier { get; set; }
    
    public void Validate(ModelStateDictionary modelState)
    {
        if (PayTo == PayTo.PLAYER && PlayerAction == PlayerAction.CURRENT_PLAYER)
        {
            modelState.AddModelError("Pay.PlayerAction", "You must select a player action when paying to/receiving from a player.");
        }
    }
}

public enum PayTo
{
    BANK,
    FREE_PARKING,
    PLAYER
}

public enum PlayerAction
{
    CURRENT_PLAYER = 0,
    SPECIFIC_PLAYER,
    LEFT,
    RIGHT,
    ALL
}

public enum PayMultiplier
{
    FIXED = 0,
    HOUSE,
    HOTEL,
    PROPERTY,
    SINGLE_DICE,
    DUEL_DICE,
    TRIPLE_DICE
}