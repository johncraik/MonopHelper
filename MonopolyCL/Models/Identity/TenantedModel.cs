using System.ComponentModel.DataAnnotations.Schema;
using MonopHelper.Authentication;

namespace MonopolyCL.Models.Identity;

public class TenantedModel
{
    public int TenantId { get; set; }
    public bool IsDeleted { get; set; }
}