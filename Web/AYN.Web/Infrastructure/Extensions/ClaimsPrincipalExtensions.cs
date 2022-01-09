using System.Security.Claims;

using AYN.Common;

namespace AYN.Web.Infrastructure.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetId(this ClaimsPrincipal user)
        => user.FindFirstValue(ClaimTypes.NameIdentifier);

    public static bool IsAdmin(this ClaimsPrincipal user)
        => user.IsInRole(GlobalConstants.AdministratorRoleName);
}
