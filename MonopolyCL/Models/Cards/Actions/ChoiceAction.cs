using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonopolyCL.Models.Cards.Actions;

public class ChoiceAction : ICardAction
{
    public int Id { get; set; }
    [DisplayName("Card Type")]
    public int CardTypeId { get; set; }
    public string CardTypeName { get; set; }
}