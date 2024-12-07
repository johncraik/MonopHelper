using System.ComponentModel.DataAnnotations.Schema;
using MonopolyCL.Models.Boards.DataModel;
using MonopolyCL.Models.Identity;

namespace MonopolyCL.Models.Game;

public class GameDM : TenantedModel
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public GAME_RULES Rules { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime LastPlayed { get; set; }
    
    public int BoardId { get; set; }
    [ForeignKey(nameof(BoardId))]
    public virtual BoardDM BoardDataModel { get; set; }
}