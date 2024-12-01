using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonopHelper.Areas.Admin.Services;

namespace MonopHelper.Areas.Admin.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly AdminService _adminService;

    public AdminController(AdminService adminService)
    {
        _adminService = adminService;
    }
    
    public IActionResult Index()
    {
        return View();
    }

    public async Task<JsonResult> GetTenants(int currentTenant)
    {
        var tenants = (await _adminService.GetTenants()).Where(t => t.Id != currentTenant)
            .ToDictionary(t => t.Id, t => t.TenantName);

        return Json(tenants);
    }

    public async Task<bool> ChangeTenant(string userId, int tenantId)
    {
        if (string.IsNullOrEmpty(userId)) return false;
        return await _adminService.ChangeUserTenant(userId, tenantId);
    }

    [HttpPost]
    public async Task<bool> SetTenantIsDeleted(int id, bool isDeleted)
    {
        var tenant = await _adminService.FindTenant(id);
        if (tenant == null) return false;

        await _adminService.SetTenantDeleted(tenant, isDeleted);
        return true;
    }
}