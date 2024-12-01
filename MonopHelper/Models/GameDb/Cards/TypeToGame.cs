using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MonopHelper.Data;

namespace MonopHelper.Models.GameDb.Cards;

[PrimaryKey(nameof(TypeId), nameof(GameId))]
public class TypeToGame : TenantedModel
{
    public int TypeId { get; set; }
    public int GameId { get; set; }
    public uint CurrentIndex { get; set; }
    
    [ForeignKey(nameof(TypeId))]
    public virtual CardType Type { get; set; }
    
    [ForeignKey(nameof(GameId))]
    public virtual CardGame Game { get; set; }
}