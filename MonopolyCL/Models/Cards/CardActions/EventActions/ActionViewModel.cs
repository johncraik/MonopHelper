using MonopolyCL.Models.Cards.CardActions.MoveActions;

namespace MonopolyCL.Models.Cards.CardActions.EventActions;

public class ActionViewModel<T>
{
    public T? Action { get; set; }
    public int CardId { get; set; }
    public bool IsAdding { get; set; }

    public ActionViewModel()
    {
    }

    public ActionViewModel(T action, int cardId, bool isAdding)
    {
        Action = action;
        CardId = cardId;
        IsAdding = isAdding;
    }
}