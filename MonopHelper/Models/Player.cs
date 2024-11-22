using System.ComponentModel.DataAnnotations.Schema;

namespace MonopHelper.Models;

public class Player
{
    public int Id { get; set; }
    
    public required string PlayerName { get; set; }
    public int DiceOne { get; set; }
    public int DiceTwo { get; set; }
    public int JailCost { get; set; }
    
    public int GameId { get; set; }
    [ForeignKey(nameof(GameId))]
    public virtual Game Game { get; set; }
}