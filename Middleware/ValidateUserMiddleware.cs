using PremierLeague_Backend.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace PremierLeague_Backend.Middleware;

public class ValidateUserMiddleware
{
    private readonly RequestDelegate _next;

    public ValidateUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUserRepository repository)
    {
        var path = context.Request.Path.Value?.ToLower() ?? "";

        // Skip static assets for performance
        if (path.Contains("/css") || path.Contains("/js") || path.Contains("/lib") || path.Contains("/assets") || path.Contains("/upload"))
        {
            await _next(context);
            return;
        }

        if (context.User.Identity?.IsAuthenticated != true)
        {
            // Redirect unauthenticated users
            bool isAnonymousPage = path == "/en/auth/login" ||
                                   path == "/en/auth/access-denied" ||
                                   path == "/";

            if (!isAnonymousPage)
            {
                context.Response.Redirect("/en/auth/login");
                return;
            }
        }
        else
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var tokenFromCookie = context.User.FindFirst("RefreshToken")?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                var user = await repository.FindUserByIdAsync(userId);

                bool isTokenValid = !string.IsNullOrEmpty(tokenFromCookie) &&
                                    await repository.ValidateRefreshTokenAsync(userId, tokenFromCookie);

                bool isInvalid = user == null ||
                                 (user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow) ||
                                 !isTokenValid;

                if (isInvalid)
                {
                    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    // Provide a specific reason if token is the issue
                    string reason = !isTokenValid ? "session_expired" : "locked";
                    context.Response.Redirect($"/en/auth/login?status={reason}");
                    return;
                }
            }
        }

        await _next(context);

        // 404 Handling
        if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
        {
            context.Response.Redirect("/en/auth/access-denied");
        }
    }
}