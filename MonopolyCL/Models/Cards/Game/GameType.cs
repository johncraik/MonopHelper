using System.ComponentModel.DataAnnotations.Schema;
using MonopolyCL.Models.Identity;

namespace MonopolyCL.Models.Cards.Game;

public class GameType : TenantedModel
{
    public int Id { get; set; }
    public uint CurrentIndex { get; set; }
    public int GameId { get; set; }
    
    public int TypeId { get; set; }
    [ForeignKey(nameof(TypeId))]
    public virtual CardType Type { get; set; }
}