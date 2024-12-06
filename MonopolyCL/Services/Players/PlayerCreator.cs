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

        var player = new Player();
        player.Name = playerDataModel.Name;
        player.TenantId = playerDataModel.TenantId;
        player.BoardIndex = gp.BoardIndex;
        player.Money = gp.Money;
        player.JaiLCost = gp.JailCost;
        player.TripleBonus = gp.TripleBonus;

        return player;
    }
}