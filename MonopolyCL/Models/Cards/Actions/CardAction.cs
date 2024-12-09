using System.ComponentModel.DataAnnotations.Schema;
using MonopolyCL.Models.Cards.Enums;
using MonopolyCL.Models.Identity;

namespace MonopolyCL.Models.Cards.Actions;

public class CardAction : TenantedModel
{
    public int Id { get; set; }
    public CARD_ACTION Action { get; set; }
    public int ActionId { get; set; }
    public int CardId { get; set; }
    [ForeignKey(nameof(CardId))]
    public virtual Card Card { get; set; }
}

public interface ICardAction
{
    
}