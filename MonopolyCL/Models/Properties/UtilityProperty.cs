using MonopolyCL.Models.Players.DataModel;
using MonopolyCL.Models.Properties;
using MonopolyCL.Models.Properties.DataModel;

namespace MonopolyCL.Models.Properties;

public class UtilityProperty : Property, IProperty
{
    public UtilityProperty()
    {
    }

    public UtilityProperty(IProperty prop, PlayerToProperty link)
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
    
    
    public int GetRent()
    {
        return 0;
    }
}