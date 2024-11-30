using Microsoft.EntityFrameworkCore;
using MonopHelper.Authentication;
using SQLitePCL;


namespace MonopHelper.Data;

public class GameDbSet<T>
    where T : class
{
    private readonly GameDbContext _context;
    private readonly ILogger<GameDbSet<T>> _logger;
    private DbSet<T> Set;
    public readonly IQueryable<T> Qry;

    public GameDbSet(GameDbContext context, UserInfo userInfo, ILogger<GameDbSet<T>> logger)
    {
        _context = context;
        _logger = logger;

        Set = context.Set<T>();
        
        try
        {
            var tableName = _context.Model.FindEntityType(typeof(T))?.GetTableName() ?? "";
            var columnName = TenantId.TenantIdName;
            var tenantId = userInfo.TenantId.ToString();
            
            Qry = Set.FromSqlRaw($"SELECT * FROM [{tableName}] WHERE {columnName} = {tenantId} OR {columnName} = 0", tableName, columnName, tenantId);
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"Error filtering db set based on tenant ID!\n{ex}");
            Qry = Set;
        }
    }

    public async Task Test()
    {
        var keys = _context.Model.FindEntityType(typeof(T))?.FindPrimaryKey()?.Properties.ToList();
        var foreignKeys = keys?.FirstOrDefault()?.GetContainingForeignKeys();

        var x = 10;
    }

    public async Task AddAsync(T model)
    {
        await Set.AddAsync(model);
        await _context.SaveChangesAsync();
        Set = _context.Set<T>();
    }
    
    public async Task AddAsync(List<T> models)
    {
        await Set.AddRangeAsync(models);
        await _context.SaveChangesAsync();
        Set = _context.Set<T>();
    }

    public async Task UpdateAsync(T model)
    {
        Set.Update(model);
        await _context.SaveChangesAsync();
        Set = _context.Set<T>();
    }
    
    public async Task UpdateAsync(List<T> models)
    {
        Set.UpdateRange(models);
        await _context.SaveChangesAsync();
        Set = _context.Set<T>();
    }

    public async Task RemoveAsync(T model)
    {
        Set.Remove(model);
        await _context.SaveChangesAsync();
        Set = _context.Set<T>();
    }
    
    public async Task RemoveAsync(List<T> models)
    {
        Set.RemoveRange(models);
        await _context.SaveChangesAsync();
        Set = _context.Set<T>();
    }
}