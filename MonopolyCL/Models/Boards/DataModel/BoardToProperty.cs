using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MonopolyCL.Models.Identity;
using MonopolyCL.Models.Properties.DataModel;

namespace MonopolyCL.Models.Boards.DataModel;

[PrimaryKey(nameof(BoardId), nameof(PropertyName), nameof(TenantId))]
public class BoardToProperty : TenantedModel
{
    public int BoardId { get; set; }
    [ForeignKey(nameof(BoardId))]
    public virtual BoardDM Board { get; set; }
    
    public string PropertyName { get; set; }
    [ForeignKey($"{nameof(PropertyName)}, {nameof(TenantId)}")]
    public virtual PropertyDM Property { get; set; }
}