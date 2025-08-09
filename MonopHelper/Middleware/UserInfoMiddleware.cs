using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MonopHelper.Authentication;
using MonopHelper.Authentication.UserClaims;

namespace MonopHelper.Middleware;

public static class UserInfoMiddlewareExtentions
{
    public static IApplicationBuilder UseUserInfo(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UserInfoMiddleware>();
    }
}

public class UserInfoMiddleware
{
    private readonly RequestDelegate _next;

    public UserInfoMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var userInfo = (UserInfo)context.RequestServices.GetRequiredService(typeof(UserInfo));
        var io = context.RequestServices.GetRequiredService<IOptions<IdentityOptions>>();
        if (!userInfo.IsSetup)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                userInfo.Name = "System";
            }
            else
            {
                userInfo.Name = context.User.FindFirst(GameClaims.DisplayNameClaim)?.Value;
                userInfo.DisplayName = userInfo.Name;
                userInfo.UserName = context.User.FindFirst(io.Value.ClaimsIdentity.EmailClaimType)?.Value;

                var tidClaim = context.User.FindFirst(GameClaims.TenantId);
                if (tidClaim == null)
                {
                    await context.SignOutAsync("Identity.Application");
                    await _next(context);
                    return;
                }

                userInfo.TenantId = int.Parse(tidClaim.Value);
                userInfo.UserId = context.User.FindFirst(io.Value.ClaimsIdentity.UserIdClaimType)?.Value;
                userInfo.ClaimsPrincipal = context.User;

                if (string.IsNullOrWhiteSpace(userInfo.UserId))
                {
                    throw new InvalidOperationException("No userid in userinfo");
                }
            }

            userInfo.IsSetup = true;
        }

        await _next(context);
    }
}