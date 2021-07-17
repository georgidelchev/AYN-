using System;
using System.Linq;
using System.Threading.Tasks;

using AYN.Common;
using AYN.Data.Models;
using AYN.Data.Models.Enumerations;
using Microsoft.AspNetCore.Identity;

namespace AYN.Data.Seeding
{
    public class AdminAccountSeeder : ISeeder
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AdminAccountSeeder(
            UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public AdminAccountSeeder()
        {
        }

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.Roles.Any())
            {
                return;
            }

            if (dbContext.Users.Any(u => u.Email == "admin@example.com"))
            {
                return;
            }

            var user = new ApplicationUser()
            {
                AccessFailedCount = 0,
                Email = "admin@example.com",
                TwoFactorEnabled = false,
                EmailConfirmed = true,
                IsBanned = false,
                IsDeleted = false,
                CreatedOn = DateTime.UtcNow,
                LockoutEnabled = false,
                PhoneNumberConfirmed = true,
                TownId = 1,
                UserName = "admin",
                About = "admin account",
                FirstName = "ADMIN",
                Gender = Gender.Other,
                LastName = "ADMIN",
                PasswordHash = "AQAAAAEAACcQAAAAELlk89l3agOGUd0KMCKZkXdbH2lGH25wOko2QJEi2oPVITqH0Isfw1jz0CM30SkPHw==",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                NormalizedUserName = "ADMIN",
            };

            user.Roles.Add(new IdentityUserRole<string>()
            {
                RoleId = dbContext.Roles
                    .FirstOrDefault(r => r.Name == GlobalConstants.AdministratorRoleName)?.Id,
            });

            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }
    }
}
