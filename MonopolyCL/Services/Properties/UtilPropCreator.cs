using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopolyCL.Data;
using MonopolyCL.Models.Properties;
using MonopolyCL.Models.Properties.DataModel;

namespace MonopolyCL.Services.Properties;

public class UtilPropCreator(BoardContext boardContext, UserInfo userInfo) : PropertyCreator(boardContext, userInfo)
{
    public override IProperty Factory(PropertyDM p, GameProperty gp) => new UtilityProperty();
}