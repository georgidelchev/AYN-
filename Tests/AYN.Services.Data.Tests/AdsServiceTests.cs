using System.Threading.Tasks;

using AYN.Data;
using AYN.Data.Models;
using AYN.Data.Models.Enumerations;
using AYN.Data.Repositories;
using AYN.Services.Data.Implementations;
using AYN.Web.ViewModels.Ads;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace AYN.Services.Data.Tests
{
    [TestFixture]
    public class AdsServiceTests
    {
        [Test]
        public async Task GetCount_ShouldReturnCorrectResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AdsTestDb").Options;

            await using var dbContext = new ApplicationDbContext(options);

            // Act
            await dbContext.Ads.AddAsync(new Ad());
            await dbContext.Ads.AddAsync(new Ad());
            await dbContext.Ads.AddAsync(new Ad());
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<Ad>(dbContext);
            var service = new AdsService(repository, null, null);

            // Assert
            Assert.AreEqual(3, service.GetCount());
        }

        [Test]
        public async Task CreateAd_ShouldBeCreatedSuccessfully()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AdsTestDb").Options;
            await using var dbContext = new ApplicationDbContext(options);

            var ad = new CreateAdInputModel
            {
                AdType = AdType.Business,
                AddressId = 1,
                CategoryId = 1,
                DeliveryTake = DeliveryTake.Buyer,
                Description = "Awesome product!",
                Name = "Shlqpki",
                Price = 12.90M,
                ProductCondition = ProductCondition.New,
                SubCategoryId = 1,
                Weight = 1,
                TownId = 1,
            };

            // Act
            await dbContext.Ads.AddAsync(new Ad());
            await dbContext.Ads.AddAsync(new Ad());
            await dbContext.Ads.AddAsync(new Ad());
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<Ad>(dbContext);
            var service = new AdsService(repository, null, null);
            await service.CreateAsync(ad, "id");

            // Assert
            Assert.AreEqual(1, service.GetCount());
        }
    }
}
