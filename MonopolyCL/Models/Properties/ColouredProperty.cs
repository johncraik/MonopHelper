using MonopolyCL.Extensions;
using MonopolyCL.Models.Properties.Enums;

namespace MonopolyCL.Models.Properties;

public class ColouredProperty : Property, IProperty
{
    public BUILT_LEVEL BuiltLevel { get; set; }
    public int BuildCost { get; set; }
    public string Colour => Set.GetDisplayName();
    public int[] Rent { get; set; }

    public int GetRent() => Rent[(int)BuiltLevel];
}