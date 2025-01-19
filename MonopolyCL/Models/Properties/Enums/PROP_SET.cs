namespace MonopolyCL.Models.Properties.Enums;

public enum PROP_SET
{
    NONE = -1,
    BROWN = 1,  //1
    BLUE = 10,     //2 
    PINK = 2,         //3 
    ORANGE = 20,    //4
    RED = 3,           //5 
    YELLOW = 30,        //6
    GREEN = 4,          //7
    DARK_BLUE = 40,    //8
    STATION = 5,        //9
    UTILITY = 6     //10
}

public static class SetExtensions
{
    public static int GetBuildCost(this PROP_SET set)
    {
        var setNum = (int)set;
        if (setNum > 9) setNum /= 10;

        return setNum > 4 ? 0 : setNum * 50;
    }

    public static List<PROP_SET> GetSetList()
    {
        return
        [
            PROP_SET.NONE, PROP_SET.BROWN, PROP_SET.BLUE, PROP_SET.PINK, PROP_SET.ORANGE, PROP_SET.RED, 
            PROP_SET.YELLOW, PROP_SET.GREEN, PROP_SET.DARK_BLUE, PROP_SET.STATION, PROP_SET.UTILITY
        ];
    }

    public static int Order(this PROP_SET set)
    {
        var setNum = (int)set;
        var second = false;
        if (setNum > 9)
        {
            setNum = (setNum / 10);
            second = true;
        }

        if ((int)set == 6)
        {
            setNum += 4;
        }
        else if(setNum > 1)
        {
            setNum += (setNum - 1);
        }

        if (second) setNum++;
        return setNum;
    }
}