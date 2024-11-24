using System.ComponentModel.DataAnnotations.Schema;

namespace MonopHelper.Models.GameDb.Cards;

public class Card
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