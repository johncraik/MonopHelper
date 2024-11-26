using System.ComponentModel.DataAnnotations.Schema;
using MonopHelper.Models.Enums;

namespace MonopHelper.Models;

public class Property
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public PropertyCol Colour { get; set; }
    
    public int PlayerId { get; set; }
    [ForeignKey(nameof(PlayerId))]
    public virtual Player Player { get; set; }

    
}