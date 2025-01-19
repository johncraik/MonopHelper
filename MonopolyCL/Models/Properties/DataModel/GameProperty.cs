using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MonopolyCL.Models.Identity;
using MonopolyCL.Models.Players;
using MonopolyCL.Models.Players.DataModel;
using MonopolyCL.Models.Properties.Enums;

namespace MonopolyCL.Models.Properties.DataModel;

public class GameProperty : TenantedModel
{
    public int Id { get; set; }
    public BUILT_LEVEL? BuiltLevel { get; set; }
    public bool IsOwned { get; set; }
    public bool IsCompleteSet { get; set; }
    public bool IsMortgaged { get; set; }
    
    public string PropertyName { get; set; }
    public int PropertyTenantId { get; set; }
    [ForeignKey($"{nameof(PropertyName)}, {nameof(PropertyTenantId)}")]
    public virtual PropertyDM Property { get; set; }
    
    public int? OwnerId { get; set; }
    [ForeignKey(nameof(OwnerId))]
    public virtual GamePlayer Player { get; set; }
    
    public int GameId { get; set; }
}