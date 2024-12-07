using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopolyCL.Models.Properties;
using MonopolyCL.Models.Properties.DataModel;

namespace MonopolyCL.Services.Properties;

public class StationPropCreator(GameDbSet<GameProperty> propSet, UserInfo userInfo) : PropertyCreator(propSet, userInfo)
{
    public override IProperty Factory(PropertyDM p, GameProperty gp) => new StationProperty
    {
        NumInSet = 1
    };
}