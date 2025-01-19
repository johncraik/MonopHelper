using Microsoft.EntityFrameworkCore;
using MonopolyCL.Models.Identity;

namespace MonopolyCL.Models.Game;

[PrimaryKey(nameof(GameId))]
public class TurnOrder : TenantedModel
{
    public int GameId { get; set; }
    public bool IsSetup { get; set; }
    public string? Order { get; set; }
    public int CurrentTurn { get; set; }

    public bool SetOrder(List<int> ids, bool bypassCheck = false)
    {
        if (ids.Count < 2 && !bypassCheck) return false;
        
        Order = (ids.Aggregate("", (ord, id) => ord += $"{id}:"))[..^1];
        return true;
    }

    public List<int> GetOrder()
    {
        var orderIds = Order.Split(':', StringSplitOptions.RemoveEmptyEntries);
        var rtnIds = new List<int>();
        foreach (var idStr in orderIds)
        {
            var success = int.TryParse(idStr, out var id);
            if(success) rtnIds.Add(id);
        }

        return rtnIds;
    }

    public int RemovePlayer(int id)
    {
        var next = GetNext();
        var ids = GetOrder();
        if (ids.Contains(id)) ids.Remove(id);

        SetOrder(ids, true);
        CurrentTurn = next;
        return ids.Count > 1 ? -1 : ids.FirstOrDefault();
    }

    public void NextPlayer()
    {
        CurrentTurn = GetNext();
    }

    public int GetNext()
    {
        var order = GetOrder();
        var currentIndex = order.IndexOf(CurrentTurn);
        var nextIndex = currentIndex + 1;

        if (nextIndex >= order.Count) nextIndex = 0;

        return order[nextIndex];
    }
}