namespace MonopolyCL.Models.Cards.Actions;

public class ActionModel
{
    public AdvanceAction? AdvanceAction { get; set; }
    public MoveAction? MoveAction { get; set; }
    public KeepAction? KeepAction { get; set; }
    public PayPlayerAction? PayPlayerAction { get; set; }
    public StreetRepairsAction? StreetRepairsAction { get; set; }
}