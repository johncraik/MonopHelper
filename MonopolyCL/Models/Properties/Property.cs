using MonopHelper.Data;
using MonopolyCL.Models.Boards;
using MonopolyCL.Models.Players;
using MonopolyCL.Models.Players.DataModel;
using MonopolyCL.Models.Properties.DataModel;
using MonopolyCL.Models.Properties.Enums;

namespace MonopolyCL.Models.Properties;

public abstract class Property
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int TenantId { get; set; }
    public PROP_TYPE Type { get; set; }
    public byte BoardIndex { get; set; }
    public int Cost { get; set; }
    public PROP_SET Set { get; set; }
    public int GameId { get; set; }
    public int? OwnerId { get; set; }
    public bool IsOwned { get; set; }
    public bool IsCompleteSet { get; set; }
    public bool IsMortgaged { get; set; }
    public bool IsInFreeParking { get; set; }
    public bool IsReserved { get; set; }
    public int ReservedAmount { get; set; }

    public Property()
    {
    }
    
    public Property(IProperty prop, PlayerToProperty link)
    {
        Id = prop.Id;
        TenantId = prop.Id;
        Type = prop.Type;
        Cost = prop.Cost;
        BoardIndex = prop.BoardIndex;
        GameId = prop.GameId;
        Name = prop.Name;
        Set = prop.Set;
        IsInFreeParking = link.IsInFreeParking;
        IsReserved = link.IsReserved;
        ReservedAmount = link.ReservedAmount;
    }

    public virtual bool Buy(Player p)
    {
        if (OwnerId != null) return false;

        OwnerId = p.GamePid;
        return true;
    }

    public virtual (bool, int) Mortgage(Player p) => OwnerId == p.GamePid ? (true, Cost / 2) : (false, 0);

    public virtual bool Transfer()
    {
        return false;
    }
}