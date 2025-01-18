
using MonopolyCL.Models.Properties.DataModel;

namespace MonopolyCL.Models.Properties;

public class StationProperty : Property, IProperty
{
    public int NumInSet { get; set; }

    public int GetRent() => 25 * (int)Math.Pow(2, (NumInSet-1));

    public async Task<bool> BuyProperty()
    {
        return false;
    }
}