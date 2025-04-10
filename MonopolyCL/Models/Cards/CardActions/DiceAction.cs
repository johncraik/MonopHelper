using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MonopolyCL.Models.Cards.CardActions;

public class DiceAction : ICardActionModel
{
    public int ActionId { get; set; }
    public int Group { get; set; }
    public CardActions Type { get; } = CardActions.DICE;
    
    [DisplayName("Convert Dice To")]
    public DiceConvert Convert { get; set; }
    [DisplayName("Player")]
    public PlayerAction PlayerAction { get; set; }
    
    public void Validate(ModelStateDictionary modelState)
    {
    }
}

public enum DiceConvert
{
    TO_DOUBLE,
    TO_TRIPLE
}