using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using AYN.Data.Seeding.Interfaces;
using AYN.Data.Seeding.Seeders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AYN.Data.Seeding;

public class ApplicationDbContextSeederBase : ISeeder
{
    private const string SeederSuccessfullyExecutedMessage = "Seeder {0} completed successfully in {1}.";

    public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        if (dbContext == null)
        {
            throw new ArgumentNullException(nameof(dbContext));
        }

        if (serviceProvider == null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        var logger = (serviceProvider
                .GetService<ILoggerFactory>() ?? throw new ArgumentNullException(nameof(serviceProvider)))
            .CreateLogger(typeof(ApplicationDbContextSeederBase));

        var seedersInstances = new List<ISeeder>
        {
            new RolesSeeder(),
            new TownsSeeder(),
            new CategoriesSeeder(),
            new EmojiSeeder(),
            new WordBlacklistSeeder(),
            new AdministratorSeeder(),
            new RegularUserSeeder(),
        };

        var stopWatch = new Stopwatch();

        foreach (var seeder in seedersInstances)
        {
            stopWatch.Reset();
            stopWatch.Start();

            await seeder.SeedAsync(dbContext, serviceProvider);
            await dbContext.SaveChangesAsync();

            stopWatch.Stop();

            var ts = stopWatch.Elapsed;
            var elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}";

            logger.LogInformation(string.Format(SeederSuccessfullyExecutedMessage, seeder.GetType().Name, elapsedTime));
        }
    }
}
