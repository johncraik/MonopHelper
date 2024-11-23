using Microsoft.EntityFrameworkCore;
using MonopHelper.Data;
using MonopHelper.Models;

namespace MonopHelper.Services.InGame;

public class PlayerService
{
    private const int JailIncrement = 20;
    
    private readonly ApplicationDbContext _context;

    public PlayerService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Player?> GetPlayer(int id)
    {
        return await _context.Players.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<int> LeaveJail(int id)
    {
        var player = await GetPlayer(id);
        if (player == null) return 0;
        
        player.JailCost += JailIncrement;
        _context.Players.Update(player);
        await _context.SaveChangesAsync();
        return player.GameId;
    }
}