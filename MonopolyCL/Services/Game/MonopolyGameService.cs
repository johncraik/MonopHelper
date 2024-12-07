using Microsoft.EntityFrameworkCore;
using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopolyCL.Models.Boards.DataModel;
using MonopolyCL.Models.Game;
using MonopolyCL.Models.Players;
using MonopolyCL.Models.Players.DataModel;
using MonopolyCL.Models.Properties;
using MonopolyCL.Models.Properties.DataModel;
using MonopolyCL.Models.Properties.Enums;
using MonopolyCL.Services.Boards;
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
    
    private readonly GameDbSet<GameDM> _gameSet;
    private readonly GameDbSet<BoardToProperty> _boardToPropSet;
    private readonly GameDbSet<PropertyDM> _propSet;
    private readonly GameDbSet<GamePlayer> _playerSet;

    public MonopolyGameService(UserInfo userInfo,
        PlayerCreator playerCreator,
        BoardCreator boardCreator,
        ColPropCreator colPropCreator,
        StationPropCreator stationPropCreator,
        UtilPropCreator utilPropCreator,
        GameDbSet<GameDM> gameSet,
        GameDbSet<BoardToProperty> boardToPropSet,
        GameDbSet<PropertyDM> propSet,
        GameDbSet<GamePlayer> playerSet)
    {
        _userInfo = userInfo;
        
        _playerCreator = playerCreator;
        _boardCreator = boardCreator;
        _colPropCreator = colPropCreator;
        _stationPropCreator = stationPropCreator;
        _utilPropCreator = utilPropCreator;
        
        _gameSet = gameSet;
        _boardToPropSet = boardToPropSet;
        _propSet = propSet;
        _playerSet = playerSet;
    }

    public async Task<MonopolyGame?> CreateGame(int boardId, List<string> playerIds, GAME_RULES rules)
    {
        //Create Game:
        var game = new GameDM
        {
            TenantId = _userInfo.TenantId,
            UserId = _userInfo.UserId,
            DateCreated = DateTime.Now,
            LastPlayed = DateTime.Now
        };
        //Add to DB (sets ID, used in properties:
        await _gameSet.AddAsync(game);
        
        //Build Players:
        var players = await BuildPlayers(playerIds, game.Id);
        if (players.Count < 2) return null; //If not 2 or more player, return null

        //Get list of properties:
        var properties = await BuildProperties(game.Id, boardId);
        if (properties.Count != 28) return null;    //If not correct number of properties, return null
        
        //Build board:
        var board = await _boardCreator.BuildBoard(boardId, properties);
        if (board == null) return null;

        return new MonopolyGame
        {
            Board = board,
            Players = players,
            Rules = rules
        };
    }
    
    public async Task<MonopolyGame?> FetchGame(int gameId)
    {
        return null;
    }


    private async Task<List<IPlayer>> BuildPlayers(List<string> playerIds, int gameId)
    {
        var players = new List<IPlayer>();
        foreach (var pid in playerIds)
        {
            var gamePlayer = await _playerSet.Query().FirstOrDefaultAsync(p => p.PlayerName == pid
                                                                               && p.GameId == gameId);
            if (gamePlayer == null)
            {
                gamePlayer = new GamePlayer
                {
                    PlayerName = pid,
                    Money = 1500,
                    BoardIndex = 0,
                    IsInJail = false,
                    JailCost = 50,
                    TripleBonus = 1000
                };
                await _playerSet.AddAsync(gamePlayer);
            }
            
            var player = await _playerCreator.BuildPlayer(gamePlayer);
            if (player != null)
            {
                players.Add(player);
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
        var propertiesInBoard = await _boardToPropSet.Query().Where(bp => bp.BoardId == boardId).ToListAsync();
        if (propertiesInBoard.Count != 28) return [];
        //Gets property data models:
        var propertyDataModels = await _propSet.Query().Where(p => propertiesInBoard.Select(bp => bp.PropertyName)
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