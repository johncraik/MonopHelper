using Microsoft.EntityFrameworkCore;
using MonopHelper.Data;
using MonopolyCL.Models.Players;
using MonopolyCL.Models.Players.DataModel;

namespace MonopolyCL.Services.Players;

public class PlayerCreator
{
    private readonly GameDbSet<PlayerDM> _playerSet;

    public PlayerCreator(GameDbSet<PlayerDM> playerSet)
    {
        _playerSet = playerSet;
    }

    public async Task<IPlayer?> BuildPlayer(GamePlayer gp)
    {
        var playerDataModel = await _playerSet.Query().FirstOrDefaultAsync(p => p.Name == gp.PlayerName);
        if (playerDataModel == null) return null;

        var player = new Player
        {
            Name = playerDataModel.Name,
            TenantId = playerDataModel.TenantId,
            BoardIndex = gp.BoardIndex,
            Money = gp.Money,
            IsInJail = gp.IsInJail,
            JaiLCost = gp.JailCost,
            TripleBonus = gp.TripleBonus
        };

        return player;
    }
}