using MonopHelper.Models.Enums;

namespace MonopHelper.Models;

public class Property
{
    public int Id { get; set; }
    public PropertyCol Colour { get; set; }

    public string GetPropertyString()
    {
        return Colour switch
        {
            PropertyCol.DarkBlue => "Dark Blue",
            _ => Colour.ToString()
        };
    }
}