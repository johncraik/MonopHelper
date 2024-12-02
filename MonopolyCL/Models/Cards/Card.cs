using System.ComponentModel.DataAnnotations.Schema;
using MonopolyCL.Models.Identity;

namespace MonopolyCL.Models.Cards;

public class Card : TenantedModel
{
    public int Id { get; set; }
    public required string Text { get; set; }
    public int? Cost { get; set; }
    public bool IsDeleted { get; set; }
    
    public int CardTypeId { get; set; }
    [ForeignKey(nameof(CardTypeId))]
    public virtual CardType CardType { get; set; }
    
    public int DeckId { get; set; }
    [ForeignKey(nameof(DeckId))]
    public virtual CardDeck CardDeck { get; set; }
}

public class CardUpload
{
    public string Text { get; set; }
    public string Cost { get; set; }
}