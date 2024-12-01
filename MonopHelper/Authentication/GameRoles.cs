using Microsoft.AspNetCore.Mvc.Rendering;

namespace MonopHelper.Authentication;

public static class GameRoles
{
    public const string Admin = "Admin";
    public const string AccessVh = "AccessVersionHistory";
    public const string ManageCards = "ManageCards";

    public static List<SelectListItem> GetRoles()
    {
        return
        [
            new SelectListItem
            {
                Text = Admin,
                Value = Admin
            },
            new SelectListItem
            {
                Text = AccessVh,
                Value = AccessVh
            },
            new SelectListItem
            {
                Text = ManageCards,
                Value = ManageCards
            }
        ];
    }
}