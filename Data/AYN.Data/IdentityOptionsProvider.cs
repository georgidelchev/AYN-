using Microsoft.AspNetCore.Identity;

using static AYN.Common.GlobalConstants;

namespace AYN.Data
{
    public static class IdentityOptionsProvider
    {
        public static void GetIdentityOptions(IdentityOptions options)
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = PasswordMinLength;
        }
    }
}
