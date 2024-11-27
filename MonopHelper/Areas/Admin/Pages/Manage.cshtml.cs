using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Construction;
using MonopHelper.Areas.Admin.Services;
using MonopHelper.Authentication;

namespace MonopHelper.Areas.Admin.Pages;

[Authorize(Roles = "Admin")]
[Area("Admin")]
public class Manage : PageModel
{
    private readonly AdminService _adminService;

    public Manage(AdminService adminService)
    {
        _adminService = adminService;
    }
    
    public new ApplicationUser User { get; set; }
    
    public async Task<IActionResult> OnGet(string id)
    {
        if (string.IsNullOrEmpty(id)) return RedirectToPage(nameof(Index));

        var user = await _adminService.GetUser(id);
        if(user == null) return RedirectToPage(nameof(Index));

        User = user;

        return Page();
    }
}