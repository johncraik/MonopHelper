namespace MonopolyCL.Extensions;

public static class NumberExtensions
{
    public static int RoundToTen(this int val)
    {
        var round = val / 10d;
        
        //Round up if 0.5:
        if (Math.Abs((round + 0.5) - (int)((val + 5) / 10d)) < 0.1) return (int)(Math.Ceiling(round)) * 10;

        //Round for all other values:
        return (int)(Math.Round(round)) * 10;
    }
}