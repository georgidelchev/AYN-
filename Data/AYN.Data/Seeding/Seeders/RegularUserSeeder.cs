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

public class RegularUserSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (userManager.Users.Any(u => u.Email == "user@example.com"))
        {
            return;
        }

        var user = new ApplicationUser
        {
            UserName = "Ivan",
            NormalizedUserName = "IVAN",
            FirstName = "Ivan",
            LastName = "Ivanov",
            Email = "user@example.com",
            NormalizedEmail = "USER@EXAMPLE.COM",
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
    }
}
