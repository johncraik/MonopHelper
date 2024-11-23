using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonopHelper.Models;

[PrimaryKey(nameof(PlayerId), nameof(PropertyId))]
public class PlayerToProperty
{
    public int PlayerId { get; set; }
    public int PropertyId { get; set; }

    [ForeignKey(nameof(PlayerId))]
    public virtual Player Player { get; set; }
    
    [ForeignKey(nameof(PropertyId))]
    public virtual Property Property { get; set; }
}