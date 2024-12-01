using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonopHelper.Areas.Admin.Services;
using MonopHelper.Authentication;

namespace MonopHelper.Areas.Admin.Pages;

[Authorize(Roles = GameRoles.Admin)]
[Area("Admin")]
public class Index : PageModel
{
    private readonly AdminService _adminService;

    public Index(AdminService adminService)
    {
        _adminService = adminService;
    }
    
    [BindProperty]
    [Required(ErrorMessage = "Please give the tenant a name!")]
    [DisplayName("Tenant Name")]
    public string NewTenantName { get; set; }
    
    public List<UserWithRoles> UsersAndRoles { get; set; }
    public List<Tenant> Tenants { get; set; }

    public async Task SetupPage()
    {
        UsersAndRoles = await _adminService.GetAllUsers();
        Tenants = await _adminService.GetTenants();
    }
    
    public async Task<IActionResult> OnGet()
    {
        await SetupPage();
        return Page();
    }

    public async Task<IActionResult> OnPostNewTenant()
    {
        await SetupPage();
        if (!ModelState.IsValid) return Page();

        var valid = await _adminService.ValidateNewTenant(NewTenantName);
        if (!valid) return Page();

        await _adminService.CreateTenant(NewTenantName);
        return RedirectToPage(nameof(Index));
    }
}