using Microsoft.EntityFrameworkCore;
using MonopHelper.Authentication;
using MonopolyCL.Data;
using MonopolyCL.Models.Players.DataModel;
using MonopolyCL.Models.Validation;

namespace MonopolyCL.Services.Players;

public class PlayerService
{
    private readonly PlayerContext _playerContext;
    private readonly UserInfo _userInfo;
    private readonly GameContext _gameContext;

    public PlayerService(PlayerContext playerContext, UserInfo userInfo, GameContext gameContext)
    {
        _playerContext = playerContext;
        _userInfo = userInfo;
        _gameContext = gameContext;
    }

    public async Task<List<PlayerDM>> GetPlayers() => await _playerContext.Players.Query()
        .OrderBy(p => p.Name).ToListAsync();
    
    public async Task<ValidationResponse> TryAddPlayer(PlayerDM player)
    {
        var inUse = await _playerContext.Players.Query().AnyAsync(p => p.Name == player.Name && p.UserId == _userInfo.UserId);
        return inUse ? new ValidationResponse(nameof(player.Name), "This player already exists") : new ValidationResponse();
    }

    public async Task<ValidationResponse> SetupPlayerTurnOrders(List<(int Id, int GPID, int Order, int D1, int D2)> players, int gameId)
    {
        //Update the turn order to include player order:
        var turnOrder = await _gameContext.TurnOrders.Query().FirstOrDefaultAsync(t => t.GameId == gameId);
        if (turnOrder == null)
            return new ValidationResponse("Input", "An unknown error occurred. Please try again");

        var playerOne = players.MaxBy(p => p.D1 + p.D2);

        var ids = new List<int>
        {
            playerOne.Id
        };
        var nextOrder = playerOne.Order + 1;
        for (var i = 0; i < players.Count - 1; i++)
        {
            var nextPlayer = players.FirstOrDefault(p => p.Order == nextOrder);
            if (nextPlayer.Id == 0)
            {
                nextPlayer = players.MinBy(p => p.Order);
            }
            ids.Add(nextPlayer.Id);
            nextOrder = nextPlayer.Order + 1;
        }
        
        var valid = turnOrder.SetOrder(ids);
        if (!valid) return new ValidationResponse("Input", "Cannot set player order");
        
        //Create player dice numbers:
        var diceNums = players.Select(p => new DiceNumbers
        {
            GamePlayerId = p.GPID, 
            DiceOne = p.D1, 
            DiceTwo = p.D2
        }).ToList();
        await _playerContext.DiceNums.AddAsync(diceNums);
        
        turnOrder.IsSetup = true;
        await _gameContext.TurnOrders.UpdateAsync(turnOrder);

        return new ValidationResponse();
    }
}