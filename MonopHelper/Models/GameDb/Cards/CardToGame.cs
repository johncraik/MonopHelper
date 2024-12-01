using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MonopHelper.Data;

namespace MonopHelper.Models.GameDb.Cards;

[PrimaryKey(nameof(CardId), nameof(GameId))]
public class CardToGame : TenantedModel
{
    public int CardId { get; set; }
    public int GameId { get; set; }
    public uint Index { get; set; }
    
    [ForeignKey(nameof(CardId))]
    public virtual Card Card { get; set; }
    
    [ForeignKey(nameof(GameId))]
    public virtual CardGame Game { get; set; }
}