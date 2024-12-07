using Microsoft.EntityFrameworkCore;
using MonopHelper.Data;
using MonopolyCL.Models.Properties;
using MonopolyCL.Models.Properties.DataModel;

namespace MonopolyCL.Services.Properties;

public abstract class PropertyCreator
{
    private readonly GameDbSet<GameProperty> _propSet;
    public abstract IProperty Factory(PropertyDM p, GameProperty gp);

    public PropertyCreator(GameDbSet<GameProperty> propSet)
    {
        _propSet = propSet;
    }
    
    public async Task<IProperty?> BuildProperty(PropertyDM p, int gameId)
    {
        var gameProp = await _propSet.Query().FirstOrDefaultAsync(gp => gp.PropertyName == p.Name);
        if (gameProp == null)
        {
            gameProp = new GameProperty
            {
                TenantId = p.TenantId,
                GameId = gameId,
                PropertyName = p.Name,
                IsMortgaged = false,
                IsCompleteSet = false,
                IsOwned = false,
                IsDeleted = false,
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