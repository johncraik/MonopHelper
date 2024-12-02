using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MonopolyCL.Models.Identity;

namespace MonopolyCL.Models.Cards;

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