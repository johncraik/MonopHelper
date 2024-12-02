using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using MonopHelper.Authentication;

namespace MonopolyCL.Models.Identity;

public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }
    public int TenantId { get; set; }
    [ForeignKey(nameof(TenantId))]
    public virtual Tenant Tenant { get; set; }
}