using System.ComponentModel.DataAnnotations.Schema;
using MonopolyCL.Extensions;
using MonopolyCL.Models.Identity;

namespace MonopolyCL.Models.Players.DataModel;

public class GamePlayer : TenantedModel
{
    private const float JailIncrement = 0.5f;
    private const int TripleIncrement = 500;
    
    public int Id { get; set; }
    public int Order { get; set; }
    public int Money { get; set; }
    public byte BoardIndex { get; set; }
    public bool IsInJail { get; set; }
    public bool IsBankrupt { get; set; }
    
    public int? JailCost { get; set; }
    public int? TripleBonus { get; set; }
    
    public int PlayerId { get; set; }
    [ForeignKey(nameof(PlayerId))]
    public virtual PlayerDM Player { get; set; }
    
    public int GameId { get; set; }

    public void IncreaseJailCost()
    {
        if(JailCost == null) return;
        JailCost = ((int)(JailCost + (JailCost * JailIncrement))).RoundToTen();
    }

    public void ResetJailCost()
    {
        if(JailCost == null) return;
        JailCost = 50;
    }

    public void IncreaseTriple()
    {
        if(TripleBonus == null) return;
        TripleBonus += TripleIncrement;
    }
}