using Microsoft.EntityFrameworkCore;
using MonopolyCL.Data;
using MonopolyCL.Dictionaries;
using MonopolyCL.Models.Properties.DataModel;
using MonopolyCL.Models.Properties.Enums;

namespace MonopolyCL.Services.Defaults.Properties;

public class PropertyDefaults
{
    private readonly GameDbContext _context;
    private readonly CsvReader<PropertyUpload> _csvReader;

    public PropertyDefaults(GameDbContext context, CsvReader<PropertyUpload> csvReader)
    {
        _context = context;
        _csvReader = csvReader;
    }

    public async Task EnsureDefaults()
    {
        var existingProps = await _context.Properties.Where(p => p.TenantId == DefaultsDictionary.MonopolyTenant).ToListAsync();
        if (existingProps.Count == 0)
        {
            var file = File.OpenRead(
                $"{Environment.CurrentDirectory}/../MonopolyCL/Services/Defaults/Properties/Properties.csv");
            var records = _csvReader.UploadFile(file);
            var properties = records!.Select(r => new PropertyDM
                {
                    TenantId = DefaultsDictionary.MonopolyTenant,
                    Name = r.Name,
                    Cost = r.Cost,
                    BoardIndex = r.Index,
                    Type = (PROP_TYPE)r.Type,
                    Set = (PROP_SET)r.Set
                }).ToList();

            await _context.Properties.AddRangeAsync(properties);
            await _context.SaveChangesAsync();
        }
    }
}