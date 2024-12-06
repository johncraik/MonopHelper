using MonopHelper.Data;
using MonopolyCL.Models.Properties;
using MonopolyCL.Models.Properties.DataModel;

namespace MonopolyCL.Services.Properties;

public class UtilPropCreator(GameDbSet<PropertyDM> propSet) : PropertyCreator(propSet)
{
    public override IProperty Factory(PropertyDM p, GameProperty gp) => new UtilityProperty();
}