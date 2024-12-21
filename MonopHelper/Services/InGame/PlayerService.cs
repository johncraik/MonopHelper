using Microsoft.EntityFrameworkCore;
using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopHelper.Models;
using MonopolyCL.Extensions;

namespace MonopHelper.Services.InGame;

public class PlayerService
{
    private const float JailIncrement = 0.5f;
    private const int TripleIncrement = 500;
    
    private readonly ApplicationDbContext _context;
    private readonly UserInfo _userInfo;

    public PlayerService(ApplicationDbContext context, UserInfo userInfo)
    {
        _context = context;
        _userInfo = userInfo;
    }

    public async Task<Player?> GetPlayer(int id)
    {
        return await _context.Players.FirstOrDefaultAsync(p => p.TenantId == _userInfo.TenantId && p.Id == id);
    }

    public async Task<int> SetNumber(int id, byte dice1, byte dice2)
    {
        var player = await GetPlayer(id);
        if (player == null) return 0;

        player.DiceOne = dice1;
        player.DiceTwo = dice2;
        _context.Players.Update(player);
        await _context.SaveChangesAsync();

        return player.Id;
    }

    public async Task<int> LeaveJail(int id)
    {
        var player = await GetPlayer(id);
        if (player == null) return 0;
        
        player.JailCost = ((int)(player.JailCost + (player.JailCost * JailIncrement))).RoundToTen();
        _context.Players.Update(player);
        await _context.SaveChangesAsync();
        
        return player.Id;
    }

    public async Task<int> ClaimTriple(int id)
    {
        var player = await GetPlayer(id);
        if (player == null) return 0;

        player.Triple += TripleIncrement;
        _context.Players.Update(player);
        await _context.SaveChangesAsync();

        return player.Id;
    }
}