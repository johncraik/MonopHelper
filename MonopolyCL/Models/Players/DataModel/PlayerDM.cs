using Microsoft.EntityFrameworkCore;
using MonopolyCL.Models.Identity;

namespace MonopolyCL.Models.Players.DataModel;

[PrimaryKey(nameof(Name), nameof(TenantId))]
public class PlayerDM : TenantedModel
{
    public string Name { get; set; }
    public string UserId { get; set; }
}