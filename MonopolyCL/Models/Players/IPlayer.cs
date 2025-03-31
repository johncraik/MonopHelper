using MonopolyCL.Models.Cards;
using MonopolyCL.Models.Players.DataModel;
using MonopolyCL.Models.Properties;

namespace MonopolyCL.Models.Players;

public interface IPlayer
{
    public int Id { get; set; }
    public int GamePid { get; set; }
    public string Name { get; internal set; }
    public int TenantId { get; internal set; }
    public int GameId { get; internal set; }
    public bool IsBankrupt { get; internal set; }
    
    public int Order { get; internal set; }
    public int Money { get; set; }
    public uint Wins { get; set; }
    public string Colour { get; set; }
    
    public (int Dice1, int Dice2)? DiceNumber { get; set; }
    public byte BoardIndex { get; set; }
    public int? JaiLCost { get; set; }
    public int? TripleBonus { get; set; }
    
    public List<IProperty>? Properties { get; set; }
    public List<Loan> Loans { get; set; }
    public List<Card> Cards { get; set; }
}