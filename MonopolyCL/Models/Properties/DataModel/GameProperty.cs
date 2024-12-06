using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MonopolyCL.Models.Identity;
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
    [ForeignKey($"{nameof(PropertyName)}, {nameof(TenantId)}")]
    public virtual PropertyDM Property { get; set; }
    
    public string? OwnerName { get; set; }
    [ForeignKey($"{nameof(OwnerName)}, {nameof(TenantId)}")]
    public virtual PlayerDM? Player { get; set; }
    
    public int GameId { get; set; }
}