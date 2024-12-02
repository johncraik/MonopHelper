namespace MonopHelper.Data;

public static class TenantId
{
    public const string TenantIdName = nameof(TenantedModel.TenantId);
}

public class TenantedModel
{
    public int TenantId { get; set; }
}