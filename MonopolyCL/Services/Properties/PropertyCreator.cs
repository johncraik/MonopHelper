using Microsoft.EntityFrameworkCore;
using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopolyCL.Models.Properties;
using MonopolyCL.Models.Properties.DataModel;
using MonopolyCL.Models.Properties.Enums;

namespace MonopolyCL.Services.Properties;

public abstract class PropertyCreator
{
    private readonly GameDbSet<GameProperty> _propSet;
    private readonly UserInfo _userInfo;
    public abstract IProperty Factory(PropertyDM p, GameProperty gp);

    public PropertyCreator(GameDbSet<GameProperty> propSet, UserInfo userInfo)
    {
        _propSet = propSet;
        _userInfo = userInfo;
    }
    
    public async Task<IProperty?> BuildProperty(PropertyDM p, int gameId)
    {
        var gameProp = await _propSet.Query().FirstOrDefaultAsync(gp => gp.PropertyName == p.Name && gp.GameId == gameId);
        if (gameProp == null)
        {
            gameProp = new GameProperty
            {
                TenantId = _userInfo.TenantId,
                GameId = gameId,
                PropertyTenantId = p.TenantId,
                PropertyName = p.Name,
                IsMortgaged = false,
                IsCompleteSet = false,
                IsOwned = false,
                IsDeleted = false,
                BuiltLevel = BUILT_LEVEL.NONE,
                OwnerName = null
            };
            await _propSet.AddAsync(gameProp);
        };
        
        var property = Factory(p, gameProp);
        
        //Set Defaults:
        property.Name = p.Name;
        property.TenantId = p.TenantId;
        property.Type = p.Type;
        property.BoardIndex = p.BoardIndex;
        property.Cost = p.Cost;
        property.Set = p.Set;
        property.GameId = gameProp.GameId;
        property.Owner = null;
        property.IsOwned = property.Owner != null;
        property.IsCompleteSet = gameProp.IsCompleteSet;
        property.IsMortgaged = gameProp.IsMortgaged;

        return property;
    }
}