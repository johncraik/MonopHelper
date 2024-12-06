using Microsoft.EntityFrameworkCore;
using MonopHelper.Data;
using MonopolyCL.Models.Properties;
using MonopolyCL.Models.Properties.DataModel;

namespace MonopolyCL.Services.Properties;

public abstract class PropertyCreator
{
    private readonly GameDbSet<PropertyDM> _propSet;
    public abstract IProperty Factory(PropertyDM p, GameProperty gp);

    public PropertyCreator(GameDbSet<PropertyDM> propSet)
    {
        _propSet = propSet;
    }
    
    public async Task<IProperty?> BuildProperty(GameProperty gp)
    {
        var propDataModel = await _propSet.Query().FirstOrDefaultAsync(p => p.Name == gp.PropertyName);
        if (propDataModel == null) return null;
        
        var property = Factory(propDataModel, gp);
        
        //Set Defaults:
        property.Name = propDataModel.Name;
        property.TenantId = propDataModel.TenantId;
        property.Type = propDataModel.Type;
        property.BoardIndex = propDataModel.BoardIndex;
        property.Cost = propDataModel.Cost;
        property.Set = propDataModel.Set;
        property.GameId = gp.GameId;
        property.Owner = null;
        property.IsOwned = property.Owner != null;
        property.IsCompleteSet = gp.IsCompleteSet;
        property.IsMortgaged = gp.IsMortgaged;

        return property;
    }
}