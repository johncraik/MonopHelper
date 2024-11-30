using Microsoft.EntityFrameworkCore;
using MonopHelper.Authentication;
using SQLitePCL;


namespace MonopHelper.Data;

public class GameDbSet<T>
    where T : class
{
    private readonly GameDbContext _context;
    private readonly ILogger<GameDbSet<T>> _logger;
    private DbSet<T> _set;
    public readonly IQueryable<T> Qry;

    public GameDbSet(GameDbContext context, UserInfo userInfo, ILogger<GameDbSet<T>> logger)
    {
        _context = context;
        _logger = logger;

        _set = context.Set<T>();
        
        try
        {
            var tableName = _context.Model.FindEntityType(typeof(T))?.GetTableName() ?? "";
            var columnName = TenantId.TenantIdName;
            var tenantId = userInfo.TenantId.ToString();
            
            Qry = _set.FromSqlRaw($"SELECT * FROM [{tableName}] WHERE {columnName} = {tenantId} OR {columnName} = 0", tableName, columnName, tenantId);
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"Error filtering db set based on tenant ID!\n{ex}");
            Qry = _set;
        }
    }

    public async Task AddAsync(T model)
    {
        await _set.AddAsync(model);
        await _context.SaveChangesAsync();
        _set = _context.Set<T>();
        
        _logger.LogDebug($"Added single object of type: {typeof(T)}");
    }
    
    public async Task AddAsync(List<T> models)
    {
        await _set.AddRangeAsync(models);
        await _context.SaveChangesAsync();
        _set = _context.Set<T>();
        
        _logger.LogDebug($"Added multiple objects of type: {typeof(T)}");
    }

    public async Task UpdateAsync(T model)
    {
        _set.Update(model);
        await _context.SaveChangesAsync();
        _set = _context.Set<T>();
        
        _logger.LogDebug($"Updated single object of type: {typeof(T)}");
    }
    
    public async Task UpdateAsync(List<T> models)
    {
        _set.UpdateRange(models);
        await _context.SaveChangesAsync();
        _set = _context.Set<T>();
        
        _logger.LogDebug($"Updated multiple objects of type: {typeof(T)}");
    }

    public async Task RemoveAsync(T model)
    {
        _set.Remove(model);
        await _context.SaveChangesAsync();
        _set = _context.Set<T>();
        
        _logger.LogDebug($"Removed single object of type: {typeof(T)}");
    }
    
    public async Task RemoveAsync(List<T> models)
    {
        _set.RemoveRange(models);
        await _context.SaveChangesAsync();
        _set = _context.Set<T>();
        
        _logger.LogDebug($"Removed multiple objects of type: {typeof(T)}");
    }
}