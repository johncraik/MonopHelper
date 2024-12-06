using MonopolyCL.Models.Properties;

namespace MonopolyCL.Models.Players;

public interface IPlayer
{
    public string Name { get; internal set; }
    public int TenantId { get; internal set; }
    public int GameId { get; internal set; }
    
    public int Money { get; set; }
    
    public (int Dice1, int Dice2)? DiceNumber { get; set; }
    public byte BoardIndex { get; set; }
    public int? JaiLCost { get; set; }
    public int? TripleBonus { get; set; }
    
    public List<IProperty>? Properties { get; set; }
}