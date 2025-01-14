using System.ComponentModel.DataAnnotations.Schema;
using MonopolyCL.Models.Boards.DataModel;
using MonopolyCL.Models.Cards;
using MonopolyCL.Models.Identity;

namespace MonopolyCL.Models.Game;

public class GameDM : TenantedModel
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Name { get; set; }
    public GAME_RULES Rules { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime LastPlayed { get; set; }
    
    public int CardGameId { get; set; }
    [ForeignKey(nameof(CardGameId))]
    public virtual CardGame CardGame { get; set; }
    
    public int BoardId { get; set; }
    [ForeignKey(nameof(BoardId))]
    public virtual BoardDM BoardDataModel { get; set; }
}