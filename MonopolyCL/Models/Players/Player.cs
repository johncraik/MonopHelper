using MonopolyCL.Models.Properties;

namespace MonopolyCL.Models.Players;

public class Player : IPlayer
{
    public string Name { get; set; }
    public int TenantId { get; set; }
    public int GameId { get; set; }
    
    public int Money { get; set; }
    
    public (int Dice1, int Dice2)? DiceNumber { get; set; }
    public byte BoardIndex { get; set; }
    public int? JaiLCost { get; set; }
    public int? TripleBonus { get; set; }
    
    public List<IProperty>? Properties { get; set; }
}