using MonopolyCL.Models.Boards;
using MonopolyCL.Models.Players;
using MonopolyCL.Models.Properties.DataModel;
using MonopolyCL.Models.Properties.Enums;

namespace MonopolyCL.Models.Properties;

public interface IProperty : IBoardSpace
{
    public string Name { get; internal set; }
    public int TenantId { get; internal set; }
    public PROP_TYPE Type { get; internal set; }
    public int Cost { get; internal set; }
    public PROP_SET Set { get; internal set; }
    public int GameId { get; internal set; }
    public Player? Owner { get; set; }
    public bool IsOwned { get; set; }
    public bool IsCompleteSet { get; set; }
    public bool IsMortgaged { get; set; }
    
    public bool Buy(Player p);
    public int GetRent();
    public (bool, int) Mortgage(Player p);
    public bool Transfer();
}