using MonopHelper.Data;
using MonopolyCL.Models.Players;
using MonopolyCL.Models.Properties.DataModel;
using MonopolyCL.Models.Properties.Enums;

namespace MonopolyCL.Models.Properties;

public abstract class Property
{
    public string Name { get; set; }
    public int TenantId { get; set; }
    public PROP_TYPE Type { get; set; }
    public byte BoardIndex { get; set; }
    public int Cost { get; set; }
    public PROP_SET Set { get; set; }
    public int GameId { get; set; }
    public Player? Owner { get; set; }
    public bool IsOwned { get; set; }
    public bool IsCompleteSet { get; set; }
    public bool IsMortgaged { get; set; }
    

    public virtual bool Buy(Player p)
    {
        if (Owner != null) return false;

        Owner = p;
        return true;
    }

    public virtual (bool, int) Mortgage(Player p) => Owner == p ? (true, Cost / 2) : (false, 0);

    public virtual bool Transfer()
    {
        return false;
    }
}