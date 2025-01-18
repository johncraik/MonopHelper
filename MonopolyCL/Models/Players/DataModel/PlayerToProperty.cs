using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MonopolyCL.Models.Properties.DataModel;

namespace MonopolyCL.Models.Players.DataModel;

[PrimaryKey(nameof(GamePlayerId), nameof(GamePropertyId))]
public class PlayerToProperty
{
    public int GamePlayerId { get; set; }
    [ForeignKey(nameof(GamePlayerId))]
    public virtual GamePlayer Player { get; set; }
    
    public int GamePropertyId { get; set; }
    [ForeignKey(nameof(GamePropertyId))]
    public virtual GameProperty Property { get; set; }
    
    public bool IsInFreeParking { get; set; }
    public bool IsReserved { get; set; }
}