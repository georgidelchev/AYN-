using System;
using System.Linq;
using System.Threading.Tasks;
using AYN.Common;
using AYN.Data.Models;
using AYN.Data.Models.Enumerations;
using AYN.Data.Seeding.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AYN.Data.Seeding.Seeders;

public class AdministratorSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        if (userManager.Users.Any(u => u.Email == "admin@example.com") ||
            !await roleManager.RoleExistsAsync(GlobalConstants.AdministratorRoleName))
        {
            return;
        }

        var user = new ApplicationUser
        {
            UserName = "Admin",
            NormalizedUserName = "ADMIN",
            FirstName = "Admin",
            LastName = "Admin",
            Email = "admin@example.com",
            NormalizedEmail = "ADMIN@EXAMPLE.COM",
            EmailConfirmed = true,
            CreatedOn = DateTime.UtcNow,
            About = string.Empty,
            Gender = Gender.Male,
            TownId = 1,
            AccessFailedCount = 0,
            TwoFactorEnabled = false,
            IsBanned = false,
            IsDeleted = false,
            LockoutEnabled = false,
            PhoneNumberConfirmed = true,
        };

        await userManager.CreateAsync(user, "123456");
        await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
    }
}
