
using MonopolyCL.Models.Properties.DataModel;

namespace MonopolyCL.Models.Properties;

public class StationProperty : Property, IProperty
{
    public int NumInSet { get; set; }

    public int GetRent() => NumInSet * 50;

    public async Task<bool> BuyProperty()
    {
        return false;
    }
}