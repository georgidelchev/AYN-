using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data;
using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Data.Repositories;
using AYN.Services.Data.Implementations;
using Microsoft.EntityFrameworkCore;

using Moq;
using NUnit.Framework;

namespace AYN.Services.Data.Tests
{
    public class SettingsServiceTests
    {
        public void GetCountShouldReturnCorrectNumber()
        {
            var repository = new Mock<IDeletableEntityRepository<Setting>>();

            repository.Setup(r => r.All()).Returns(new List<Setting>
                                                        {
                                                            new Setting(),
                                                            new Setting(),
                                                            new Setting(),
                                                        }.AsQueryable());

            var service = new SettingsService(repository.Object);

            Assert.AreEqual(3, service.GetCount());

            repository.Verify(x => x.All(), Times.Once);
        }

        public async Task GetCountShouldReturnCorrectNumberUsingDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "SettingsTestDb").Options;

            using var dbContext = new ApplicationDbContext(options);

            dbContext.Settings.Add(new Setting());
            dbContext.Settings.Add(new Setting());
            dbContext.Settings.Add(new Setting());

            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<Setting>(dbContext);
            var service = new SettingsService(repository);

            Assert.AreEqual(3, service.GetCount());
        }
    }
}
