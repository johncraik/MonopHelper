using System.ComponentModel.DataAnnotations.Schema;
using MonopolyCL.Models.Identity;

namespace MonopolyCL.Models.Players.DataModel;

public class GamePlayer : TenantedModel
{
    public int Id { get; set; }
    public int Order { get; set; }
    public int Money { get; set; }
    public byte BoardIndex { get; set; }
    public bool IsInJail { get; set; }
    
    public int? JailCost { get; set; }
    public int? TripleBonus { get; set; }
    
    public int PlayerId { get; set; }
    [ForeignKey(nameof(PlayerId))]
    public virtual PlayerDM Player { get; set; }
    
    public int GameId { get; set; }
}