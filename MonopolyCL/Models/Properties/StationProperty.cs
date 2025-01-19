
using MonopolyCL.Models.Players.DataModel;
using MonopolyCL.Models.Properties.DataModel;

namespace MonopolyCL.Models.Properties;

public class StationProperty : Property, IProperty
{
    public int NumInSet { get; set; }

    public int GetRent() => 25 * (int)Math.Pow(2, (NumInSet-1));

    public StationProperty()
    {
    }

    public StationProperty(IProperty prop, PlayerToProperty link)
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
    
    public async Task<bool> BuyProperty()
    {
        return false;
    }
}