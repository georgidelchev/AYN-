using System;
using System.Threading.Tasks;

namespace AYN.Data.Seeding.Interfaces;

public interface ISeeder
{
    Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider);
}
