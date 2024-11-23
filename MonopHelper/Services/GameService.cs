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
        if (p == null || p.Contains(NewGamePlayers.NoPlayers)) return false;
        
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
        var game = await _context.Games.FirstOrDefaultAsync(g => !g.IsDeleted && g.Id == id && g.UserId == userId);
        var players = await _context.Players.Where(p => p.GameId == id).ToListAsync();

        return (game, players);
    }

    public async Task<List<LoadGame>> FetchGames(DateTime? date = null, string search = "")
    {
        var qry = _context.Games.Where(g => !g.IsDeleted);

        if (date != null) qry = qry.Where(g => g.DateCreated.Date == date);
        if (!string.IsNullOrEmpty(search))
            qry = qry.Where(g => g.GameName.Contains(search));

        var loadGames = new List<LoadGame>();
        foreach (var game in await qry.ToListAsync())
        {
            var players = await _context.Players.Where(p => p.GameId == game.Id).ToListAsync();
            loadGames.Add(new LoadGame
            {
                Game = game,
                Players = players
            });
        }

        return loadGames;
    }

    public async Task<List<InGamePlayer>> GetInGamePlayers(List<Player> players)
    {
        var inGamePlayers = new List<InGamePlayer>();
        foreach (var p in players)
        {
            var properties = await _context.PlayerToProperties.Include(pp => pp.Property)
                .Where(pp => pp.PlayerId == p.Id).Select(pp => pp.Property)
                .OrderBy(prop => prop.Colour).ToListAsync();
            var loans = await _context.PlayerToLoans.Include(pl => pl.Loan)
                .Where(pl => pl.PlayerId == p.Id).Select(pl => pl.Loan)
                .OrderBy(l => l.Repaid).ToListAsync();
            
            inGamePlayers.Add(new InGamePlayer
            {
                Player = p,
                Properties = properties,
                Loans = loans
            });
        }

        return inGamePlayers;
    }

    public async Task DeleteGame(int id, string userId)
    {
        var (game, players) = await FetchGame(id, userId);
        if(game != null) await DeleteGame(game, players);
    }
    
    public async Task DeleteGame(Game game, List<Player>? players)
    {
        game.IsDeleted = true;
        _context.Games.Update(game);
        if(players is { Count: > 0 }) _context.Players.RemoveRange(players);

        await _context.SaveChangesAsync();
    }
}