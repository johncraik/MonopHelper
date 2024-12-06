using MonopHelper.Data;
using MonopolyCL.Models.Properties;
using MonopolyCL.Models.Properties.DataModel;
using MonopolyCL.Models.Properties.Enums;

namespace MonopolyCL.Services.Properties;

public class ColPropCreator(GameDbSet<PropertyDM> propSet) : PropertyCreator(propSet)
{
    public override IProperty Factory(PropertyDM p, GameProperty gp)
    {
        var prop = new ColouredProperty
        {
            BuiltLevel = gp.BuiltLevel ?? BUILT_LEVEL.NONE,
            BuildCost = p.Set.GetBuildCost()
        };

        prop.Rent = prop.Set switch
        {
            PROP_SET.BROWN => prop.BoardIndex == 3 ? [4, 20, 60, 180, 320, 450, 900] : [2, 10, 30, 90, 160, 250, 500],
            PROP_SET.BLUE => prop.BoardIndex == 9 ? [8, 40, 100, 300, 450, 600, 1200] : [6, 30, 90, 270, 400, 550, 1100],
            PROP_SET.PINK => prop.BoardIndex == 14 ? [12, 60, 180, 500, 700, 900, 1800] : [10, 50, 150, 450, 625, 750, 1500],
            PROP_SET.ORANGE => prop.BoardIndex == 19 ? [16, 80, 220, 600, 800, 1000, 2000] : [14, 70, 200, 550, 750, 950, 1900],
            PROP_SET.RED => prop.BoardIndex == 24 ? [20, 100, 300, 750, 925, 1100, 2200] : [18, 90, 250, 700, 875, 1050, 2100],
            PROP_SET.YELLOW => prop.BoardIndex == 29 ? [24, 120, 360, 850, 1025, 1200, 2400] : [22, 110, 330, 800, 975, 1150, 2300],
            PROP_SET.GREEN => prop.BoardIndex == 34 ? [28, 150, 450, 1000, 1200, 1400, 2800] : [26, 130, 390, 900, 1100, 1275, 2550],
            PROP_SET.DARK_BLUE => prop.BoardIndex == 39 ? [50, 200, 600, 1400, 1700, 2000, 4000] : [35, 175, 500, 1100, 1300, 1500, 3000],
            _ => [0, 0, 0, 0, 0, 0, 0]
        };
        
        return prop;
    }
}