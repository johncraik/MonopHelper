using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonopolyCL.Models.Players.DataModel;

[PrimaryKey(nameof(GamePlayerId))]
public class DiceNumbers
{
    public int GamePlayerId { get; set; }
    public int DiceOne { get; set; }
    public int DiceTwo { get; set; }
    
    [ForeignKey(nameof(GamePlayerId))]
    public virtual GamePlayer Player { get; set; }
}