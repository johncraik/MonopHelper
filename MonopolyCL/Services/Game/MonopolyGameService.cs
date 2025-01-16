using Microsoft.EntityFrameworkCore;
using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopolyCL.Data;
using MonopolyCL.Models.Boards.DataModel;
using MonopolyCL.Models.Cards.ViewModels;
using MonopolyCL.Models.Game;
using MonopolyCL.Models.Players;
using MonopolyCL.Models.Players.DataModel;
using MonopolyCL.Models.Properties;
using MonopolyCL.Models.Properties.DataModel;
using MonopolyCL.Models.Properties.Enums;
using MonopolyCL.Models.Validation;
using MonopolyCL.Services.Boards;
using MonopolyCL.Services.Cards;
using MonopolyCL.Services.Players;
using MonopolyCL.Services.Properties;

namespace MonopolyCL.Services.Game;

public class MonopolyGameService
{
    private readonly UserInfo _userInfo;
    
    private readonly PlayerCreator _playerCreator;
    private readonly BoardCreator _boardCreator;
    private readonly ColPropCreator _colPropCreator;
    private readonly StationPropCreator _stationPropCreator;
    private readonly UtilPropCreator _utilPropCreator;

    private readonly CardGameService _cardGameService;
    private readonly BoardContext _boardContext;
    private readonly PlayerContext _playerContext;
    private readonly GameContext _gameContext;

    public MonopolyGameService(UserInfo userInfo,
        PlayerCreator playerCreator,
        BoardCreator boardCreator,
        ColPropCreator colPropCreator,
        StationPropCreator stationPropCreator,
        UtilPropCreator utilPropCreator,
        CardGameService cardGameService,
        BoardContext boardContext,
        PlayerContext playerContext,
        GameContext gameContext)
    {
        _userInfo = userInfo;
        
        _playerCreator = playerCreator;
        _boardCreator = boardCreator;
        _colPropCreator = colPropCreator;
        _stationPropCreator = stationPropCreator;
        _utilPropCreator = utilPropCreator;
        _cardGameService = cardGameService;
        _boardContext = boardContext;
        _playerContext = playerContext;
        _gameContext = gameContext;
    }

    public async Task<List<MonopolyGame>> GetGames()
    {
        var gameIds = await _gameContext.Games.Query().Select(g => g.Id).ToListAsync();
        var games = new List<MonopolyGame>();
        foreach (var g in gameIds)
        {
            var game = await FetchGame(g);
            if(game != null) games.Add(game);
        }

        return games;
    }

    public async Task<TurnOrder?> GetGameTurnOrder(int id) =>
        await _gameContext.TurnOrders.Query().FirstOrDefaultAsync(t => t.GameId == id);

    public async Task CreateGameTurnOrder(TurnOrder turnOrder)
    {
        await _gameContext.TurnOrders.AddAsync(turnOrder);
    }

    public async Task<ValidationResponse<MonopolyGame>> CreateGame(string name, int boardId, int deckId, List<int> playerIds, GAME_RULES rules)
    {
        //Create Card Game:
        var cardGame = await _cardGameService.CreateGame(deckId);
        if (cardGame == null) return new ValidationResponse<MonopolyGame>
        {
            Response = new ValidationResponse(false, "All", "Could not create game")
        };
        
        //Create Game:
        var game = new GameDM
        {
            Name = name,
            TenantId = _userInfo.TenantId,
            UserId = _userInfo.UserId,
            BoardId = boardId,
            CardGameId = cardGame.Game.Id,
            Rules = rules,
            DateCreated = DateTime.Now,
            LastPlayed = DateTime.Now
        };
        //Add to DB (sets ID, used in properties:
        await _gameContext.Games.AddAsync(game);
        
        //Create Turn Order:
        var order = new TurnOrder
        {
            GameId = game.Id,
            TenantId = _userInfo.TenantId,
            IsSetup = false
        };
        await _gameContext.TurnOrders.AddAsync(order);

        return new ValidationResponse<MonopolyGame>
        {
            ReturnObj = await BuildGame(game, playerIds, cardGame),
            Response = new ValidationResponse()
        };
    }
    
    public async Task<MonopolyGame?> FetchGame(int gameId)
    {
        var game = await _gameContext.Games.Query().Include(g => g.CardGame).FirstOrDefaultAsync(g => g.Id == gameId);
        if (game == null) return null;
        
        return await BuildGame(game);
    }

    private async Task<MonopolyGame?> BuildGame(GameDM game, List<int>? playerIds = null, CardGameViewModel? cardGame = null)
    {
        //Build Players:
        var players = await BuildPlayers(game.Id, playerIds);
        if (players.Count < 2) return null; //If not 2 or more player, return null

        //Get list of properties:
        var properties = await BuildProperties(game.Id, game.BoardId);
        if (properties.Count != 28) return null;    //If not correct number of properties, return null
        
        //Build board:
        var board = await _boardCreator.BuildBoard(game.BoardId, properties);
        if (board == null) return null;

        return new MonopolyGame
        {
            Game = game,
            Board = board,
            Players = players,
            Cards = cardGame ?? await _cardGameService.FetchGame(game.CardGameId)
        };
    }


    private async Task<List<IPlayer>> BuildPlayers(int gameId, List<int>? playerIds)
    {
        var players = new List<IPlayer>();
        
        var gamePlayers = await _playerContext.GamePlayers.Query().Where(p => p.GameId == gameId).ToListAsync();
        switch (gamePlayers.Count)
        {
            case 0 when playerIds != null:
            {
                foreach (var gamePlayer in playerIds.Select(pid => new GamePlayer
                         {
                             TenantId = _userInfo.TenantId,
                             GameId = gameId,
                             PlayerId = pid,
                             Order = 0,
                             Money = 1500,
                             BoardIndex = 0,
                             IsInJail = false,
                             JailCost = 50,
                             TripleBonus = 1000
                         }))
                {
                    await _playerContext.GamePlayers.AddAsync(gamePlayer);
                
                    var player = await _playerCreator.BuildPlayer(gamePlayer);
                    if (player != null) players.Add(player);
                }

                break;
            }
            case > 0:
            {
                foreach (var gp in gamePlayers)
                {
                    var player = await _playerCreator.BuildPlayer(gp);
                    if (player != null) players.Add(player);
                }

                break;
            }
        }

        return players;
    }
    

    /// <summary>
    /// Uses property factory to create a list of properties on the board:
    /// </summary>
    /// <param name="gameId"></param>
    /// <param name="boardId"></param>
    /// <returns></returns>
    private async Task<List<IProperty>> BuildProperties(int gameId, int boardId)
    {
        //Gets properties in board:
        var propertiesInBoard = await _boardContext.BoardsToProperties.Query().Where(bp => bp.BoardId == boardId).ToListAsync();
        if (propertiesInBoard.Count != 28) return [];
        //Gets property data models:
        var propertyDataModels = await _boardContext.Properties.Query().Where(p => propertiesInBoard.Select(bp => bp.PropertyName)
            .Contains(p.Name)).ToListAsync();
        
        //Build properties using factory:
        var properties = new List<IProperty>();
        foreach (var p in propertyDataModels)
        {
            //Choose factory based on property type:
            var property = p.Type switch
            {
                PROP_TYPE.COLOURED => await _colPropCreator.BuildProperty(p, gameId),
                PROP_TYPE.STATION => await _stationPropCreator.BuildProperty(p, gameId),
                PROP_TYPE.UTILITY => await _utilPropCreator.BuildProperty(p, gameId),
                _ => null
            };
            
            //Add to list if properties are not null:
            if(property != null) properties.Add(property);
        }

        return properties;
    }
}