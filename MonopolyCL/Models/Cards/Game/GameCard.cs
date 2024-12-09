using System.ComponentModel.DataAnnotations.Schema;
using MonopolyCL.Models.Identity;

namespace MonopolyCL.Models.Cards.Game;

public class GameCard : TenantedModel
{
    public int Id { get; set; }
    public uint Index { get; set; }
    public int GameId { get; set; }
    public int CardId { get; set; }
    [ForeignKey(nameof(CardId))]
    public virtual Card Card { get; set; }
}