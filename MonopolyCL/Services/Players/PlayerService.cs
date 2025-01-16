using Microsoft.EntityFrameworkCore;
using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopolyCL.Data;
using MonopolyCL.Models.Players.DataModel;
using MonopolyCL.Models.Validation;

namespace MonopolyCL.Services.Players;

public class PlayerService
{
    private readonly PlayerContext _context;
    private readonly UserInfo _userInfo;

    public PlayerService(PlayerContext context, UserInfo userInfo)
    {
        _context = context;
        _userInfo = userInfo;
    }

    public async Task<List<PlayerDM>> GetPlayers() =>
        await _context.Players.Query().Where(p => p.UserId == _userInfo.UserId).ToListAsync();
    
    public async Task<ValidationResponse> TryAddPlayer(PlayerDM player)
    {
        var inUse = await _context.Players.Query().AnyAsync(p => p.Name == player.Name && p.UserId == _userInfo.UserId);
        return inUse ? new ValidationResponse(false, nameof(player.Name), "This player already exists") : new ValidationResponse();
    }
}