using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MonopolyCL.Models.Cards;
using MonopolyCL.Models.Identity;

namespace MonopolyCL.Models.Players.DataModel;

[PrimaryKey(nameof(PlayerId), nameof(CardId))]
public class PlayerToCard : TenantedModel
{
    public int PlayerId { get; set; }
    [ForeignKey(nameof(PlayerId))] 
    public virtual GamePlayer Player { get; set; }

    public int CardId { get; set; }
    [ForeignKey(nameof(CardId))]
    public virtual Card Card { get; set; }
}