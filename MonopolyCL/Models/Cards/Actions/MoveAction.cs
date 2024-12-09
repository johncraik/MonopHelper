using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace MonopolyCL.Models.Cards.Actions;

public class MoveAction : ICardAction
{
    public int Id { get; set; }
    [DisplayName("Move Amount")]
    public int MoveAmount { get; set; }
    [DisplayName("Move Forwards?")]
    public bool IsForward { get; set; }
}