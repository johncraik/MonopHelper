namespace MonopHelper.Models.Enums;

public enum PropertyCol
{
    Brown,
    Blue,
    Pink,
    Orange,
    Red,
    Yellow,
    Green,
    DarkBlue,
    Station,
    Utility
}

public static class PropCol
{
    public static string GetPropertyString(PropertyCol col)
    {
        return col switch
        {
            PropertyCol.DarkBlue => "Dark Blue",
            _ => col.ToString()
        };
    }
}