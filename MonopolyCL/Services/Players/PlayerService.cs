using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MonopHelper.Authentication;
using MonopolyCL.Data;
using MonopolyCL.Models.Players;
using MonopolyCL.Models.Players.DataModel;
using MonopolyCL.Models.Validation;

namespace MonopolyCL.Services.Players;

public class PlayerService
{
    private readonly PlayerContext _playerContext;
    private readonly PlayerCreator _playerCreator;
    private readonly UserInfo _userInfo;
    private readonly GameContext _gameContext;
    private readonly ILogger<PlayerService> _logger;

    public PlayerService(PlayerContext playerContext,
        PlayerCreator playerCreator,
        UserInfo userInfo, 
        GameContext gameContext, 
        ILogger<PlayerService> logger)
    {
        _playerContext = playerContext;
        _playerCreator = playerCreator;
        _userInfo = userInfo;
        _gameContext = gameContext;
        _logger = logger;
    }

    public async Task<List<PlayerDM>> GetPlayers() => await _playerContext.Players.Query()
        .OrderBy(p => p.Name).ToListAsync();

    public async Task<ValidationResponse<dynamic>> GetGamePlayer(int id, bool includeDice = false)
    {
        if (includeDice)
        {
            var diceWithPlayer = await _playerContext.DiceNums.Query().Include(d => d.Player)
                .FirstOrDefaultAsync(d => d.GamePlayerId == id);
            if (diceWithPlayer == null)
                return new ValidationResponse<dynamic>("CurrentPlayer", "Could not find player!");

            return new ValidationResponse<dynamic>
            {
                ReturnObj = diceWithPlayer
            };
        }

        var player = await _playerContext.GamePlayers.Query().FirstOrDefaultAsync(p => p.Id == id);
        if (player == null) return new ValidationResponse<dynamic>("CurrentPlayer", "Could not find player!");
        
        return new ValidationResponse<dynamic>
        {
            ReturnObj = player
        };
    }

    public async Task<ValidationResponse<dynamic>> UpdateGamePlayer(dynamic player, bool updateBoth = false)
    {
        try
        {
            if (player.GetType() == typeof(GamePlayer))
            {
                //Update game player:
                await _playerContext.GamePlayers.UpdateAsync((GamePlayer)player);
                return new ValidationResponse<dynamic>
                {
                    ReturnObj = (GamePlayer)player
                };
            }
            
            if(player.GetType() == typeof(DiceNumbers))
            {
                if (updateBoth)
                {
                    var dice = (DiceNumbers)player;
                    var p = dice.Player;

                    await _playerContext.GamePlayers.UpdateAsync(p);
                    await _playerContext.DiceNums.UpdateAsync(dice);
                    return new ValidationResponse<dynamic>
                    {
                        ReturnObj = dice
                    };
                }
                
                //Update dice numbers:
                await _playerContext.DiceNums.UpdateAsync((DiceNumbers)player);
                return new ValidationResponse<dynamic>
                {
                    ReturnObj = (DiceNumbers)player
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Cannot update game player/dice numbers! --- {ex}");
        }

        return new ValidationResponse<dynamic>("CurrentPlayer", "An unknown error occured. Please try again");
    }

    public async Task<IPlayer?> BuildPlayer(GamePlayer gamePlayer) => await _playerCreator.BuildPlayer(gamePlayer);
    
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
        turnOrder.CurrentTurn = playerOne.Id;
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

    public async Task<bool> EndTurn(int gameId)
    {
        var turn = await _gameContext.TurnOrders.Query().FirstOrDefaultAsync(t => t.GameId == gameId);
        if (turn == null) return false;
        
        turn.NextPlayer();
        await _gameContext.TurnOrders.UpdateAsync(turn);
        return true;
    }

    public async Task PayLoans(int playerId, byte? type = 5, uint? total = 0)
    {
        var loans = await _playerContext.PlayerLoans.Query().Where(l => l.PlayerId == playerId).ToListAsync();
        if(loans.Count == 0) return;

        var splitAmount = total ?? 0;
        if ((type == null || type < 0) && total is > 0)
        {
            splitAmount = (uint)Math.Round((double)total / loans.Count);
        }

        foreach (var loan in loans)
        {
            var repay = type > 0 ? loan.Pass(type ?? 0) : (int)splitAmount;
            loan.RepaidAmount += repay;
            if (loan.RepaidAmount < loan.Amount) continue;
            
            loan.RepaidAmount = loan.Amount;
            loan.Repaid = true;
        }

        await _playerContext.PlayerLoans.UpdateAsync(loans);
    }

    public async Task<ValidationResponse> NewLoan(int playerId, uint amount)
    {
        if (amount <= 0) return new ValidationResponse("NewLoanInput.Amount", "Please enter a valid loan amount");

        var playerExists = await _playerContext.GamePlayers.Query().AnyAsync(p => p.Id == playerId);
        if (!playerExists) return new ValidationResponse("NewLoanInput.Amount", "An unknown error occurred");
        
        var loan = new Loan
        {
            TenantId = _userInfo.TenantId,
            PlayerId = playerId,
            Amount = (int)amount
        };
        await _playerContext.PlayerLoans.AddAsync(loan);
        return new ValidationResponse();
    }
}