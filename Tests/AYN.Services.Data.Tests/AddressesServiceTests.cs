using System;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data;
using AYN.Data.Models;
using AYN.Data.Repositories;
using AYN.Services.Data.Implementations;
using AYN.Services.Data.Interfaces;
using AYN.Services.Mapping;
using AYN.Web.ViewModels.Addresses;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace AYN.Services.Data.Tests;

[TestFixture]
public class AddressesServiceTests
{
    private ApplicationDbContext dbContext;
    private EfDeletableEntityRepository<Address> addressesRepository;
    private Mock<ITownsService> mockedITownsService;
    private DbContextOptionsBuilder<ApplicationDbContext> options;
    private IAddressesService addressesService;

    [SetUp]
    public void SetUp()
    {
        this.options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        this.dbContext = new ApplicationDbContext(this.options.Options);
        this.addressesRepository = new EfDeletableEntityRepository<Address>(this.dbContext);
        this.mockedITownsService = new Mock<ITownsService>();
        this.addressesService = new AddressesService(this.addressesRepository, this.mockedITownsService.Object);

        AutoMapperConfig.RegisterMappings(typeof(GetAddressesViewModel).Assembly, typeof(Address).Assembly);
    }

    [Test]
    public async Task GetAllByTownIdAsync_ShouldReturnAllAddressesSuccessfully()
    {
        for (var i = 1; i <= 10; i++)
        {
            var town = new Town()
            {
                Name = $"Town{i}",
            };
            town.Addresses.Add(new Address()
            {
                Name = $"Address{i}",
            });

            await this.dbContext.Towns.AddAsync(town);
            await this.dbContext.SaveChangesAsync();
        }

        var allAddressesByTownId = await this.addressesService.GetAllByTownIdAsync<GetAddressesViewModel>(1);

        Assert.AreEqual(1, allAddressesByTownId.Count());
    }

    [Test]
    public async Task GetAllByTownIdAsKeyValuePairsAsync_ShouldReturnAllAddressesSuccessfully()
    {
        var town = new Town()
        {
            Name = "Town1",
        };

        for (var i = 1; i <= 10; i++)
        {
            town.Addresses.Add(new Address()
            {
                Name = $"Address{i}",
            });
        }

        var addressesAsKvp = await this.addressesService.GetAllByTownIdAsKeyValuePairsAsync(1);

        var counter = 1;
        foreach (var (key, value) in addressesAsKvp)
        {
            Assert.AreEqual(counter, key);
            Assert.AreEqual($"Address{counter}", value);

            counter++;
        }
    }
}
