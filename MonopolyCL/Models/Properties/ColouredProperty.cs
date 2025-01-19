using MonopolyCL.Extensions;
using MonopolyCL.Models.Players;
using MonopolyCL.Models.Players.DataModel;
using MonopolyCL.Models.Properties.Enums;

namespace MonopolyCL.Models.Properties;

public class ColouredProperty : Property, IProperty
{
    public BUILT_LEVEL BuiltLevel { get; set; }
    public int BuildCost { get; set; }
    public string Colour => Set.GetDisplayName();
    public int[] Rent { get; set; }

    public int GetRent() => Rent[(int)BuiltLevel];

    public ColouredProperty()
    {
    }
    
    public ColouredProperty(IProperty prop, PlayerToProperty link)
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
    
    public override bool Buy(Player p)
    {
        return true;
    }
}