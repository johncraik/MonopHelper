namespace MonopHelper.Authentication;

public class Tenant
{
    public int Id { get; set; }
    public required string TenantName { get; set; }
    public DateTime DateCreated { get; set; }
    public bool IsDeleted { get; set; }
}