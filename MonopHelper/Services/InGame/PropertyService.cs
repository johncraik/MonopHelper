using Microsoft.EntityFrameworkCore;
using MonopHelper.Authentication;
using MonopHelper.Data;
using MonopHelper.Models;
using MonopHelper.Models.Enums;

namespace MonopHelper.Services.InGame;

public class PropertyService
{
    private readonly ApplicationDbContext _context;
    private readonly UserInfo _userInfo;

    public PropertyService(ApplicationDbContext context, UserInfo userInfo)
    {
        _context = context;
        _userInfo = userInfo;
    }

    private readonly List<PropertyCol> _propertyColours =
    [
        PropertyCol.Brown, PropertyCol.Blue,
        PropertyCol.Pink, PropertyCol.Orange,
        PropertyCol.Red, PropertyCol.Yellow,
        PropertyCol.Green, PropertyCol.DarkBlue,
        PropertyCol.Station, PropertyCol.Utility
    ];

    public PropertyCol? FindPropertyColour(string col)
    {
        var success = int.TryParse(col, out var colNum);
        if (success)
        {
            return (PropertyCol)colNum;
        }

        return null;
    }
    
    public async Task<List<Property>> GetPlayerProperties(int playerId)
    {
        return await _context.Properties.Where(pp => pp.TenantId == _userInfo.TenantId && pp.PlayerId == playerId)
            .OrderBy(pp => pp.Colour).ToListAsync();
    }

    public async Task<List<PropertyCol>> GetPlayerUnusedColours(int playerId)
    {
        var propCols = (await GetPlayerProperties(playerId)).Select(p => p.Colour);
        return _propertyColours.Where(p => !propCols.Contains(p)).ToList();
    }

    public async Task AddProperty(PropertyCol col, int playerId)
    {
        var property = new Property
        {
            TenantId = _userInfo.TenantId,
            PlayerId = playerId,
            Colour = col
        };
        
        await _context.Properties.AddAsync(property);
        await _context.SaveChangesAsync();
    }
}