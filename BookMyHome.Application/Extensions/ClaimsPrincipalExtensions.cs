using System.Security.Claims;

namespace BookMyHome.Application.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(
        this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(value, out var userId))
        {
            throw new InvalidOperationException(
                "The authenticated user ID is missing or invalid.");
        }

        return userId;
    }
}