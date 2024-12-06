namespace MonopolyCL.Models.Properties.Enums;

public enum PROP_SET
{
    BROWN = 1,          //2, 4
    BLUE = 10,          //6, 6, 8
    PINK = 2,           //10, 10, 12
    ORANGE = 20,        //14, 14, 16
    RED = 3,            //18, 18, 20
    YELLOW = 30,        //22, 22, 24
    GREEN = 4,          //26, 26, 28
    DARK_BLUE = 40,     //35, 50
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