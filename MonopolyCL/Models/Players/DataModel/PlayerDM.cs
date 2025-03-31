using Microsoft.EntityFrameworkCore;
using MonopolyCL.Models.Identity;

namespace MonopolyCL.Models.Players.DataModel;

[PrimaryKey(nameof(Id))]
public class PlayerDM : TenantedModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public uint Wins { get; set; }
    public string UserId { get; set; }
    public string Colour { get; set; }
}