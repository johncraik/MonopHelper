using Microsoft.EntityFrameworkCore;
using MonopHelper.Data;
using MonopHelper.Models;
using MonopHelper.Models.ViewModels;

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

    public async Task<bool> AddPlayers(string? p, int game)
    {
        if (p == null) return false;
        
        var players = new NewGamePlayers(p);
        var addPlayers = players.Players.Select(player => new Player
            {
                PlayerName = player,
                DiceOne = 0,
                DiceTwo = 0,
                GameId = game,
                JailCost = 50
            }).ToList();

        await _context.Players.AddRangeAsync(addPlayers);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<(Game? game, List<Player> players)> FetchGame(int id, string userId)
    {
        var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);
        var players = await _context.Players.Where(p => p.GameId == id).ToListAsync();

        return (game, players);
    }

    public async Task DeleteGame(int id, string userId)
    {
        var (game, players) = await FetchGame(id, userId);
        if(game != null) await DeleteGame(game, players);
    }
    
    public async Task DeleteGame(Game game, List<Player>? players)
    {
        _context.Games.Remove(game);
        if(players is { Count: > 0 }) _context.Players.RemoveRange(players);

        await _context.SaveChangesAsync();
    }
}