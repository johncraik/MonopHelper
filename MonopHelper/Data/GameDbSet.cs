using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MonopHelper.Authentication;


namespace MonopHelper.Data;

public class GameDbSet<T>
    where T : class
{
    private readonly GameDbContext _context;
    private readonly UserInfo _userInfo;
    private readonly ILogger<GameDbSet<T>> _logger;
    
    private DbSet<T> _set;
    private string _tenantQry;
    private IQueryable<T> _qry;

    public GameDbSet(GameDbContext context, UserInfo userInfo, ILogger<GameDbSet<T>> logger)
    {
        _context = context;
        _userInfo = userInfo;
        _logger = logger;

        _set = context.Set<T>();
        FilterDbSet();
    }

    private void FilterDbSet()
    {
        try
        {
            var tfp = TenantParams();

            _tenantQry = $"SELECT * FROM [{tfp[0]}] WHERE ({tfp[1]} = {tfp[2]} OR {tfp[1]} = 0)";
            _qry = _set.FromSqlRaw(_tenantQry, tfp[0], tfp[1], tfp[2]);
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"Error filtering db set ({typeof(T)}) based on tenant ID!\n{ex}");
            _qry = _set;
        }
    }

    private string[] TenantParams()
    {
        try
        {
            var tableName = _context.Model.FindEntityType(typeof(T))?.GetTableName() ?? "";
            var tenantIdName = TenantId.TenantIdName;
            var tenant = _userInfo.TenantId.ToString();

            return [tableName, tenantIdName, tenant];
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"Error filtering db set ({typeof(T)}) based on tenant ID!\n{ex}");
            return [];
        }
    }

    public IQueryable<T> Query(bool rtnDeleted = false)
    {
        if (rtnDeleted) return _qry;
        
        try
        {
            var tfp = TenantParams();
            return _set.FromSqlRaw($"{_tenantQry} AND IsDeleted = False", tfp[0], tfp[1], tfp[2]);
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"Cannot filter query (type: {typeof(T)}) based on tenant ID and deleted entities!\n{ex}");
            return _qry;
        }
    }

    public async Task AddAsync(T model)
    {
        await _set.AddAsync(model);
        await _context.SaveChangesAsync();
        _set = _context.Set<T>();
        FilterDbSet();
        
        _logger.LogDebug($"Added single object of type: {typeof(T)}");
    }
    
    public async Task AddAsync(List<T> models)
    {
        await _set.AddRangeAsync(models);
        await _context.SaveChangesAsync();
        _set = _context.Set<T>();
        FilterDbSet();
        
        _logger.LogDebug($"Added multiple objects of type: {typeof(T)}");
    }

    public async Task UpdateAsync(T model)
    {
        _set.Update(model);
        await _context.SaveChangesAsync();
        _set = _context.Set<T>();
        FilterDbSet();
        
        _logger.LogDebug($"Updated single object of type: {typeof(T)}");
    }
    
    public async Task UpdateAsync(List<T> models)
    {
        _set.UpdateRange(models);
        await _context.SaveChangesAsync();
        _set = _context.Set<T>();
        FilterDbSet();
        
        _logger.LogDebug($"Updated multiple objects of type: {typeof(T)}");
    }

    public async Task RemoveAsync(T model)
    {
        _set.Remove(model);
        await _context.SaveChangesAsync();
        _set = _context.Set<T>();
        FilterDbSet();
        
        _logger.LogDebug($"Removed single object of type: {typeof(T)}");
    }
    
    public async Task RemoveAsync(List<T> models)
    {
        _set.RemoveRange(models);
        await _context.SaveChangesAsync();
        _set = _context.Set<T>();
        FilterDbSet();
        
        _logger.LogDebug($"Removed multiple objects of type: {typeof(T)}");
    }
}