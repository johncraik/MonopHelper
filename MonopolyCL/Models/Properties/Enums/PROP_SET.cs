namespace MonopolyCL.Models.Properties.Enums;

public enum PROP_SET
{
    BROWN = 1,
    BLUE = 10,        
    PINK = 2,           
    ORANGE = 20,
    RED = 3,            
    YELLOW = 30,        
    GREEN = 4,          
    DARK_BLUE = 40,    
    STATION = 5,
    UTILITY = 6
}

public static class SetExtensions
{
    public static int GetBuildCost(this PROP_SET set)
    {
        var setNum = (int)set;
        if (setNum > 9) setNum /= 10;

        return setNum > 4 ? 0 : setNum * 50;
    }
}