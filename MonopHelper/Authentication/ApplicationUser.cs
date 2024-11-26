using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MonopHelper.Authentication;

public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }
    public int TenantId { get; set; }
    [ForeignKey(nameof(TenantId))]
    public virtual Tenant Tenant { get; set; }
}