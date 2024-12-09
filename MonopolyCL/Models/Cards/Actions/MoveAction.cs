using Microsoft.EntityFrameworkCore;

namespace MonopolyCL.Models.Cards.Actions;

public class MoveAction : ICardAction
{
    public int Id { get; set; }
    public int MoveAmount { get; set; }
    public bool IsForward { get; set; }
}