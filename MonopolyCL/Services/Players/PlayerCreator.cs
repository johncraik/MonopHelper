using Microsoft.EntityFrameworkCore;
using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopolyCL.Data;
using MonopolyCL.Models.Players;
using MonopolyCL.Models.Players.DataModel;

namespace MonopolyCL.Services.Players;

public class PlayerCreator
{
    private readonly PlayerContext _context;
    private readonly UserInfo _userInfo;

    public PlayerCreator(PlayerContext context, UserInfo userInfo)
    {
        _context = context;
        _userInfo = userInfo;
    }

    public async Task<IPlayer?> BuildPlayer(GamePlayer gp)
    {
        var playerDataModel = await _context.Players.Query().FirstOrDefaultAsync(p => p.Id == gp.PlayerId);
        if (playerDataModel == null) return null;

        var diceNums = await _context.DiceNums.Query().FirstOrDefaultAsync(d => d.GamePlayerId == gp.Id);
        
        var player = new Player
        {
            Id = playerDataModel.Id,
            GamePid = gp.Id,
            Name = playerDataModel.Name,
            TenantId = playerDataModel.TenantId,
            Order = gp.Order,
            BoardIndex = gp.BoardIndex,
            Money = gp.Money,
            IsInJail = gp.IsInJail,
            JaiLCost = gp.JailCost,
            TripleBonus = gp.TripleBonus,
            GameId = gp.GameId,
            DiceNumber = (diceNums?.DiceOne ?? 0, diceNums?.DiceTwo ?? 0)
        };

        return player;
    }
}