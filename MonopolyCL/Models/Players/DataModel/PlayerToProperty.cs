using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MonopolyCL.Models.Properties.DataModel;

namespace MonopolyCL.Models.Players.DataModel;

public class PlayerToProperty
{
    public int Id { get; set; }
    public int GamePlayerId { get; set; }
    [ForeignKey(nameof(GamePlayerId))]
    public virtual GamePlayer Player { get; set; }
    
    public int GamePropertyId { get; set; }
    [ForeignKey(nameof(GamePropertyId))]
    public virtual GameProperty Property { get; set; }
    
    public bool IsInFreeParking { get; set; }
    public bool IsReserved { get; set; }
    public int ReservedAmount { get; set; }
}