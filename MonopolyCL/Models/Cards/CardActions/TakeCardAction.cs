using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MonopolyCL.Models.Cards.CardActions;

public class TakeCardAction : ICardActionModel
{
    public int ActionId { get; set; }
    public int Group { get; set; }
    public CardActions Type { get; } = CardActions.TAKE_CARD;
    
    [DisplayName("Card Types")]
    [MinLength(1)]
    public int[] CardTypeIds { get; set; }
    [DisplayName("Player Action")]
    public PlayerAction PlayerAction { get; set; }
    
    public void Validate(ModelStateDictionary modelState)
    {
        
    }
}