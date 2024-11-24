using Microsoft.EntityFrameworkCore;
using MonopHelper.Data;
using MonopHelper.Models;

namespace MonopHelper.Services.InGame;

public class PlayerService
{
    private const int JailIncrement = 20;
    private const int TripleIncrement = 500;
    
    private readonly ApplicationDbContext _context;

    public PlayerService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Player?> GetPlayer(int id)
    {
        return await _context.Players.FirstOrDefaultAsync(p => p.Id == id);
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
        
        player.JailCost += JailIncrement;
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