using Microsoft.AspNetCore.Mvc;
using MonopHelper.Authentication;
using MonopolyCL.Models.Game;
using MonopolyCL.Models.Players;
using MonopolyCL.Models.Players.DataModel;
using MonopolyCL.Models.Validation;
using MonopolyCL.Services.Cards;
using MonopolyCL.Services.Game;
using MonopolyCL.Services.Players;
using MonopolyCL.Services.Properties;
using Newtonsoft.Json;

namespace MonopHelper.Controllers;

public class TurnController : Controller
{
    private readonly MonopolyGameService _gameService;
    private readonly PlayerService _playerService;
    private readonly PropertyService _propertyService;
    private readonly CardGameService _cardGameService;
    private readonly UserInfo _userInfo;

    public TurnController(MonopolyGameService gameService, 
        PlayerService playerService, 
        PropertyService propertyService,
        CardGameService cardGameService,
        UserInfo userInfo)
    {
        _gameService = gameService;
        _playerService = playerService;
        _propertyService = propertyService;
        _cardGameService = cardGameService;
        _userInfo = userInfo;
    }

    public async Task<IActionResult> GetPlayerPartial(int id, int gameId)
    {
        var res = await _playerService.GetGamePlayer(id);
        IPlayer? model = null;
        if (res.IsValid && res.ReturnObj != null && res.ReturnObj?.GetType() == typeof(GamePlayer))
        {
            var game = await _gameService.FetchGame(gameId);
            if (game != null) model = game.Players.FirstOrDefault(p => p.GamePid == ((GamePlayer)res.ReturnObj!).Id);
        }

        return PartialView("TurnBased/_PlayerPartial", model);
    }

    public async Task<IActionResult> GetAlertPartial(int id, int gameId)
    {
        var res = await _playerService.GetGamePlayer(id);
        IPlayer? player = null;
        var model = new GameAlert();
        if (res.IsValid && res.ReturnObj != null && res.ReturnObj?.GetType() == typeof(GamePlayer))
        {
            var game = await _gameService.FetchGame(gameId);
            if (game != null)
            {
                player = game.Players.FirstOrDefault(p => p.GamePid == ((GamePlayer)res.ReturnObj!).Id);
                model = _gameService.GetGameAlert(player);
            } 
        }

        return PartialView("TurnBased/_AlertPartial", model);
    }

    public async Task<IActionResult> GetCardPartial(int gameId, int typeId)
    {
        var model = await _cardGameService.GetCard(gameId, typeId);
        return PartialView("TurnBased/_CardPartial", model);
    }
    
    
    [HttpPost]
    public async Task<string> ChangeNumber(int id, int d1, int d2)
    {
        if (d1 > 6 || d1 < 1 || d2 > 6 || d2 < 1) 
            return JsonConvert.SerializeObject(new ValidationResponse("CurrentPlayer", "One of the dice numbers entered is not a valid dice number"));

        var res = await _playerService.GetGamePlayer(id, true);
        if (!res.IsValid || res.ReturnObj == null || res.ReturnObj?.GetType() != typeof(DiceNumbers)) 
            return JsonConvert.SerializeObject(res);

        var dice = (DiceNumbers)res.ReturnObj!;
        dice.DiceOne = d1;
        dice.DiceTwo = d2;

        var updateRes = await _playerService.UpdateGamePlayer(dice);
        return JsonConvert.SerializeObject(updateRes);
    }

    [HttpPost]
    public async Task<string> LeaveJail(int id)
    {
        //Get Player:
        var res = await _playerService.GetGamePlayer(id);
        if (!res.IsValid || res.ReturnObj == null || res.ReturnObj?.GetType() != typeof(GamePlayer))
            return JsonConvert.SerializeObject(res);

        //Increase jail cost:
        var player = (GamePlayer)res.ReturnObj!;
        player.IncreaseJailCost();
        
        //Update player:
        var updateRes = await _playerService.UpdateGamePlayer(player);
        return JsonConvert.SerializeObject(updateRes);
    }

    [HttpPost]
    public async Task<string> ResetJail(int id)
    {
        //Get Player:
        var res = await _playerService.GetGamePlayer(id);
        if (!res.IsValid || res.ReturnObj == null || res.ReturnObj?.GetType() != typeof(GamePlayer))
            return JsonConvert.SerializeObject(res);

        //Increase jail cost:
        var player = (GamePlayer)res.ReturnObj!;
        player.ResetJailCost();
        
        //Update player:
        var updateRes = await _playerService.UpdateGamePlayer(player);
        return JsonConvert.SerializeObject(updateRes);
    }

    [HttpPost]
    public async Task<string> ClaimTriple(int id)
    {
        //Get Player:
        var res = await _playerService.GetGamePlayer(id);
        if (!res.IsValid || res.ReturnObj == null || res.ReturnObj?.GetType() != typeof(GamePlayer))
            return JsonConvert.SerializeObject(res);

        //Increase jail cost:
        var player = (GamePlayer)res.ReturnObj!;
        player.IncreaseTriple();
        
        //Update player:
        var updateRes = await _playerService.UpdateGamePlayer(player);
        return JsonConvert.SerializeObject(updateRes);
    }

    [HttpPost]
    public async Task<string> Unmortgage(int id)
    {
        var res = await _propertyService.UnmortgageProperty(id);
        return JsonConvert.SerializeObject(res);
    }

    public async Task<string> UnlockReservation(int propId, int playerId)
    {
        var res = await _propertyService.UnlockReservation(propId, playerId);
        return JsonConvert.SerializeObject(res);
    }
}