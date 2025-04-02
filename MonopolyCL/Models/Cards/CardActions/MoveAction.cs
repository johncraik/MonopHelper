namespace MonopolyCL.Models.Cards.CardActions;

public class MoveAction
{
    public int Value { get; set; }  //Advance => board index, !Advance => spaces.
    public bool IsForward { get; set; } = true;
    public bool IsAdvance { get; set; }
    public bool IsAllPlayers { get; set; }
}