using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonopHelper.Areas.Admin.Services;
using MonopHelper.Authentication;

namespace MonopHelper.Areas.Admin.Pages;

[Authorize(Roles = "Admin")]
[Area("Admin")]
public class Index : PageModel
{
    private readonly AdminService _adminService;

    public Index(AdminService adminService)
    {
        _adminService = adminService;
    }
    
    public List<UserWithRoles> UsersAndRoles { get; set; }
    public List<Tenant> Tenants { get; set; }
    
    public async Task<IActionResult> OnGet()
    {
        UsersAndRoles = await _adminService.GetAllUsers();
        Tenants = await _adminService.GetTenants();

        return Page();
    }
}