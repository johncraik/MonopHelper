using Microsoft.EntityFrameworkCore;
using MonopolyCL.Models.Identity;
using MonopolyCL.Models.Properties.Enums;

namespace MonopolyCL.Models.Properties.DataModel;

[PrimaryKey(nameof(Name), nameof(TenantId))]
public class PropertyDM : TenantedModel
{
    public string Name { get; set; }
    public byte BoardIndex { get; set; }
    public PROP_TYPE Type { get; set; }
    public int Cost { get; set; }
    public PROP_SET Set { get; set; }
}

public class PropertyUpload
{
    public string Name { get; set; }
    public int Cost { get; set; }
    public byte Index { get; set; }
    public int Type { get; set; }
    public int Set { get; set; }
}