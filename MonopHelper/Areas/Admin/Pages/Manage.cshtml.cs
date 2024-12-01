using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonopHelper.Areas.Admin.Services;
using MonopHelper.Authentication;

namespace MonopHelper.Areas.Admin.Pages;

[Authorize(Roles = GameRoles.Admin)]
[Area("Admin")]
public class Manage : PageModel
{
    private readonly AdminService _adminService;

    public Manage(AdminService adminService)
    {
        _adminService = adminService;
    }
    
    public UserWithRoles UserWithRoles { get; set; }
    
    public List<SelectListItem> Roles { get; set; }
    [BindProperty]
    [Required]
    [DisplayName("Select Role")]
    public string SelectedRole { get; set; }
    [BindProperty]
    public string UserId { get; set; }

    public async Task<bool> SetupPage(string id)
    {
        var user = await _adminService.GetUser(id);
        if(user == null) return false;

        UserWithRoles = user;
        Roles = GameRoles.GetRoles()
            .Where(r => !UserWithRoles.Roles.Select(ur => ur.Name).Contains(r.Text))
            .ToList();
        return true;
    }
    
    public async Task<IActionResult> OnGet(string id)
    {
        if (string.IsNullOrEmpty(id)) return RedirectToPage(nameof(Index));

        var setup = await SetupPage(id);
        if (!setup) return new UnauthorizedResult();

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        var setup = await SetupPage(UserId);
        if (!setup) return new UnauthorizedResult();
        
        var roleExists = await _adminService.RoleExists(SelectedRole);
        if (roleExists)
        {
            await _adminService.AddRoleToUser(UserId, SelectedRole);
        }

        return RedirectToPage(nameof(Manage), new {id = UserId});
    }
}