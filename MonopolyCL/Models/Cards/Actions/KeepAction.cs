namespace MonopolyCL.Models.Cards.Actions;

public class KeepAction : ICardAction
{
    public int Id { get; set; }
    public int? PlayerId { get; set; }
}