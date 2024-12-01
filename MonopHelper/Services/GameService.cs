using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopHelper.Models;
using MonopHelper.Models.ViewModels;
using MonopHelper.Services.InGame;

namespace MonopHelper.Services;

public class GameService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;
    private readonly PropertyService _propertyService;
    private readonly LoanService _loanService;
    private readonly UserInfo _userInfo;

    public GameService(ApplicationDbContext context, IConfiguration config, PropertyService propertyService,
        LoanService loanService, UserInfo userInfo)
    {
        _context = context;
        _config = config;
        _propertyService = propertyService;
        _loanService = loanService;
        _userInfo = userInfo;
    }

    public async Task<int> CreateGame(string name)
    {
        var game = new Game
        {
            UserId = _userInfo.UserId,
            TenantId = _userInfo.TenantId,
            GameName = name,
            DateCreated = DateTime.UtcNow
        };

        await _context.Games.AddAsync(game);
        await _context.SaveChangesAsync();

        return game.Id;
    }

    public async Task<(bool, string)> AddPlayers(string? p, int game)
    {
        if (p == null || p.Contains(NewGamePlayers.NoPlayers)) return (false, "Please add some players to this game!");
        
        var players = new NewGamePlayers(p);
        var addPlayers = players.Players.Select(player => new Player
            {
                PlayerName = player,
                DiceOne = 0,
                DiceTwo = 0,
                GameId = game,
                JailCost = 50,
                Triple = 1000,
                TenantId = _userInfo.TenantId
            }).ToList();

        await _context.Players.AddRangeAsync(addPlayers);
        await _context.SaveChangesAsync();
        return addPlayers.Count > 1 ? (true, "") : (false, "You can't play yourself!");
    }

    public async Task<(Game? game, List<Player> players)> FetchGame(int id)
    {
        var game = await _context.Games.FirstOrDefaultAsync(g => g.TenantId == _userInfo.TenantId 
                                                                 && !g.IsDeleted && g.Id == id && g.UserId == _userInfo.UserId);
        var players = await _context.Players.Where(p => p.TenantId == _userInfo.TenantId 
                                                        && p.GameId == id).ToListAsync();

        return (game, players);
    }

    public async Task<List<LoadGame>> FetchGames(DateTime? date = null, string search = "")
    {
        var qry = _context.Games.Include(g => g.Players)
            .Where(g => g.TenantId == _userInfo.TenantId && !g.IsDeleted && g.UserId == _userInfo.UserId);

        if (date != null && date.Value.Date != new DateTime(1,1,1).Date) qry = qry.Where(g => g.DateCreated.Date == date);

        var games = await qry.ToListAsync();
        if (!string.IsNullOrEmpty(search))
        {
            //Search game name:
            var oldQry = qry;
            qry = oldQry.Where(g => g.GameName.ToLower().Contains(search.ToLower()));
            
            //Search players:
            var plyQry = oldQry.Select(g => g.Players);
            var gamesWithPlayers = new List<Game>();
            foreach (var players in plyQry)
            {
                if (players.Select(p => p.PlayerName.ToLower())
                    .Any(player => player.Contains(search, StringComparison.CurrentCultureIgnoreCase)))
                {
                    gamesWithPlayers.Add(players.First().Game);
                }
            }
            
            games = (await qry.ToListAsync()).Concat(gamesWithPlayers).ToList();
        }

        var loadGames = new List<LoadGame>();
        foreach (var game in games)
        {
            var players = await _context.Players.Where(p => p.TenantId == _userInfo.TenantId 
                                                            && p.GameId == game.Id).ToListAsync();
            loadGames.Add(new LoadGame
            {
                Game = game,
                Players = players
            });
        }

        return loadGames.OrderByDescending(g => g.Game.DateCreated).ToList();
    }

    public async Task<InGamePlayer?> GetInGamePlayer(int playerId)
    {
        var player = await _context.Players.FirstOrDefaultAsync(p => p.TenantId == _userInfo.TenantId 
                                                                     && p.Id == playerId);
        if (player == null) return null;
        
        var properties = await _propertyService.GetPlayerProperties(player.Id);
        var loans = await _loanService.GetPlayerLoans(player.Id);
            
        return new InGamePlayer
        {
            Player = player,
            Properties = properties,
            Loans = loans
        };
    }
    
    public async Task<List<InGamePlayer>> GetInGamePlayers(List<Player> players)
    {
        var inGamePlayers = new List<InGamePlayer>();
        foreach (var p in players)
        {
            var properties = await _propertyService.GetPlayerProperties(p.Id);
            var loans = await _loanService.GetPlayerLoans(p.Id);
            
            inGamePlayers.Add(new InGamePlayer
            {
                Player = p,
                Properties = properties,
                Loans = loans
            });
        }

        return inGamePlayers;
    }

    public async Task DeleteGame(int id)
    {
        var (game, players) = await FetchGame(id);
        if(game != null) await DeleteGame(game, players);
    }
    
    public async Task DeleteGame(Game game, List<Player>? players)
    {
        game.IsDeleted = true;
        _context.Games.Update(game);

        await _context.SaveChangesAsync();
    }
}