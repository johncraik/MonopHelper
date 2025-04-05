using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MonopolyCL.Models.Cards.CardActions;

public class MoveAction : ICardActionModel
{
    public int ActionId { get; set; }
    public int Group { get; set; }
    public CardActions Type { get; } = CardActions.MOVE;
    
    [Range(0, 39)]
    public int Value { get; set; }  //Advance => board index, !Advance => spaces.
    [DisplayName("Moving Forwards?")]
    public bool IsForward { get; set; } = true;
    [DisplayName("Advance to Space on Board?")]
    public bool IsAdvance { get; set; }
    [DisplayName("Affects All Players?")]
    public bool IsAllPlayers { get; set; }
}