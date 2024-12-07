using MonopolyCL.Models.Identity;

namespace MonopolyCL.Models.Game;

public class GameDM : TenantedModel
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime LastPlayed { get; set; }
}