using Microsoft.EntityFrameworkCore;
using MonopolyCL.Data;
using MonopolyCL.Extensions;
using MonopolyCL.Models.Players.DataModel;
using MonopolyCL.Models.Properties.Enums;
using MonopolyCL.Models.Validation;
using SQLitePCL;

namespace MonopolyCL.Services.Properties;

public class PropertyService
{
    private readonly BoardContext _boardContext;
    private readonly PlayerContext _playerContext;

    public PropertyService(BoardContext boardContext, PlayerContext playerContext)
    {
        _boardContext = boardContext;
        _playerContext = playerContext;
    }

    public async Task<bool> HandInProperty(int propId, int playerId)
    {
        var prop = await _boardContext.GameProperties.Query().FirstOrDefaultAsync(p => p.Id == propId);
        if (prop == null) return false;
        
        var playerLink = new PlayerToProperty
        {
            GamePlayerId = playerId,
            GamePropertyId = propId,
            IsInFreeParking = true
        };
        await _playerContext.PlayersToProperties.AddAsync(playerLink);
        return true;
    }

    public async Task<bool> MortgageProperty(int propId, int playerId)
    {
        var prop = await _boardContext.GameProperties.Query().FirstOrDefaultAsync(p => p.Id == propId);
        if (prop == null) return false;

        prop.IsMortgaged = true;
        prop.IsOwned = true;
        prop.OwnerId = playerId;
        await _boardContext.GameProperties.UpdateAsync(prop);
        return true;
    }

    public async Task<ValidationResponse> UnmortgageProperty(int propId)
    {
        var property = await _boardContext.GameProperties.Query().FirstOrDefaultAsync(p => p.Id == propId);
        if (property == null) return new ValidationResponse("", "Unable to un-mortgage this property");

        property.IsMortgaged = false;
        property.IsOwned = false;
        property.OwnerId = null;
        await _boardContext.GameProperties.UpdateAsync(property);
        return new ValidationResponse();
    }

    public async Task<bool> ReserveProperty(int propId, int playerId)
    {
        var prop = await _boardContext.GameProperties.Query().FirstOrDefaultAsync(p => p.Id == propId);
        if (prop == null) return false;

        var link = new PlayerToProperty
        {
            GamePlayerId = playerId,
            GamePropertyId = propId,
            IsReserved = true
        };
        await _playerContext.PlayersToProperties.AddAsync(link);
        return true;
    }

    public async Task<ValidationResponse> UnlockReservation(int propId, int playerId)
    {
        var property = await _boardContext.GameProperties.Query().FirstOrDefaultAsync(p => p.Id == propId);
        if (property == null) return new ValidationResponse("", "Cannot find property");

        var link = await _playerContext.PlayersToProperties.Query()
            .FirstOrDefaultAsync(l => l.GamePropertyId == propId && l.GamePlayerId == playerId && l.IsReserved);
        if (link == null) return new ValidationResponse("", "Unable to unlock this reserved property");

        await _playerContext.PlayersToProperties.RemoveAsync(link);
        return new ValidationResponse();
    }

    public async Task<List<(string Txt, string Val)>> GetFreeParkingDropDown(int playerId, int gameId)
    {
        var links = await _playerContext.PlayersToProperties.Query()
            .Where(l => l.GamePlayerId == playerId && l.IsInFreeParking)
            .Select(l => l.GamePropertyId).ToListAsync();

        var properties = (await _boardContext.GameProperties.Query().Include(p => p.Property)
            .Where(p => p.GameId == gameId).ToListAsync()).DistinctBy(p => p.Property.Set);
        
        return properties.Where(p => !links.Contains(p.Id)).OrderBy(p => p.Property.Set.Order())
            .Select(p => (p.Property.Set.GetDisplayName(), p.Id.ToString())).ToList();
    }

    public async Task<List<(string Txt, string Val)>> GetMortgageDropdown(int playerId, int gameId)
    {
        var properties = await _boardContext.GameProperties.Query()
            .Where(p => p.GameId == gameId && p.OwnerId == null).ToListAsync();
        return properties.OrderBy(p => p.Property.Set.Order()).ThenBy(p => p.Property.Cost)
            .Select(p => (p.PropertyName, p.Id.ToString())).ToList();
    }

    public async Task<List<(string Txt, string Val)>> GetReservationDropdown(int playerId, int gameId)
    {
        var links = await _playerContext.PlayersToProperties.Query()
            .Where(l => l.IsReserved)
            .Select(l => l.GamePropertyId).ToListAsync();

        var properties = await _boardContext.GameProperties.Query().Include(p => p.Property)
            .Where(p => p.GameId == gameId).ToListAsync();

        return properties.Where(p => !links.Contains(p.Id)).OrderBy(p => p.Property.Set.Order())
            .ThenBy(p => p.Property.Cost).Select(p => (p.PropertyName, p.Id.ToString())).ToList();
    }
}