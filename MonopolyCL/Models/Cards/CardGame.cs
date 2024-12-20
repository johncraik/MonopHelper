using System.ComponentModel.DataAnnotations.Schema;
using MonopolyCL.Models.Identity;

namespace MonopolyCL.Models.Cards;

public class CardGame : TenantedModel
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime LastPlayed { get; set; }
    public bool IsDeleted { get; set; }
    
    public int DeckId { get; set; }
    [ForeignKey(nameof(DeckId))]
    public virtual CardDeck Deck { get; set; }
}