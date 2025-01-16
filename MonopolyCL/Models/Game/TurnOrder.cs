using Microsoft.EntityFrameworkCore;
using MonopolyCL.Models.Identity;

namespace MonopolyCL.Models.Game;

[PrimaryKey(nameof(GameId))]
public class TurnOrder : TenantedModel
{
    public int GameId { get; set; }
    public bool IsSetup { get; set; }
    public string? Order { get; set; }

    public bool SetOrder(List<int> ids)
    {
        if (ids.Count < 2) return false;
        
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
}