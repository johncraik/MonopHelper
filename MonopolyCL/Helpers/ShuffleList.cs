namespace MonopolyCL.Helpers;

public class ShuffleList<T>
{
    private Random Rng;
    private static int _seed = new Random().Next();
    
    public ShuffleList()
    {
        Rng = new Random(_seed);
        _seed = Rng.Next();
    }
    
    public List<T> Shuffle(List<T> list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = Rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }

        return list;
    }
}