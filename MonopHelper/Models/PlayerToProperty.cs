using Microsoft.EntityFrameworkCore;

namespace MonopHelper.Models;

[PrimaryKey(nameof(PlayerId), nameof(PropertyId))]
public class PlayerToProperty
{
    public int PlayerId { get; set; }
    public int PropertyId { get; set; }
}