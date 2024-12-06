using MonopolyCL.Models.Properties;
using MonopolyCL.Models.Properties.DataModel;

namespace MonopolyCL.Models.Properties;

public class UtilityProperty : Property, IProperty
{
    public int GetRent()
    {
        return 0;
    }
}