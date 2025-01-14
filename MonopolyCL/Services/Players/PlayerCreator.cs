using Microsoft.EntityFrameworkCore;
using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopolyCL.Models.Players;
using MonopolyCL.Models.Players.DataModel;

namespace MonopolyCL.Services.Players;

public class PlayerCreator
{
    private readonly GameDbSet<PlayerDM> _playerSet;
    private readonly UserInfo _userInfo;

    public PlayerCreator(GameDbSet<PlayerDM> playerSet, UserInfo userInfo)
    {
        _playerSet = playerSet;
        _userInfo = userInfo;
    }

    public async Task<IPlayer?> BuildPlayer(GamePlayer gp)
    {
        var playerDataModel = await _playerSet.Query().FirstOrDefaultAsync(p => p.Name == gp.PlayerName 
            && p.UserId == _userInfo.UserId);
        if (playerDataModel == null) return null;

        var player = new Player
        {
            Name = playerDataModel.Name,
            TenantId = playerDataModel.TenantId,
            Order = gp.Order,
            BoardIndex = gp.BoardIndex,
            Money = gp.Money,
            IsInJail = gp.IsInJail,
            JaiLCost = gp.JailCost,
            TripleBonus = gp.TripleBonus,
            GameId = gp.GameId
        };

        return player;
    }
}