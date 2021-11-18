using System.Security.Claims;

namespace SoarBeyond.Shared.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static bool IsAuthenticated(this ClaimsPrincipal principal)
    {
        return principal.Identity?.IsAuthenticated ?? false;
    }

    public static bool IsNotAuthenticated(this ClaimsPrincipal principal)
    {
        return !IsAuthenticated(principal);
    }

    public static string GetUserEmail(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.Email);
    }

    /// <summary>
    /// Tries to get the UserId from the ClaimsPrincipal
    /// </summary>
    /// <param name="principal"></param>
    /// <returns>UserId if it's found, or null if not found</returns>
    public static string GetUserId(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public static string GetUserName(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.Name);
    }

    // From PrincipalExtensions source (no need for Identity namespace 'using')
    private static string FindFirstValue(this ClaimsPrincipal principal, string claimType)
    {
        var claim = principal.FindFirst(claimType);
        return claim?.Value;
    }
}