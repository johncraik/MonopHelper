using Microsoft.EntityFrameworkCore;
using MonopHelper.Data;
using MonopHelper.Models;

namespace MonopHelper.Services;

public class GameService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;

    public GameService(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<int> CreateGame(string name)
    {
        var game = new Game
        {
            UserId = _config["init_userId"] ?? "default",
            GameName = name,
            DateCreated = DateTime.UtcNow
        };

        await _context.Games.AddAsync(game);
        await _context.SaveChangesAsync();

        return game.Id;
    }

    public async Task<Game?> FetchGame(int id, string userId)
    {
        return await _context.Games.FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);
    }
}