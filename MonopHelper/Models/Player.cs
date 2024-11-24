using System.ComponentModel.DataAnnotations.Schema;

namespace MonopHelper.Models;

public class Player
{
    public int Id { get; set; }
    
    public required string PlayerName { get; set; }
    public byte DiceOne { get; set; }
    public byte DiceTwo { get; set; }
    public int JailCost { get; set; }
    
    public int GameId { get; set; }
    [ForeignKey(nameof(GameId))]
    public virtual Game Game { get; set; }
    
    public virtual List<Property> Properties { get; set; }
    public virtual List<Loan> Loans { get; set; }
}