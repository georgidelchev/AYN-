using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AYN.Data;
using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Data.Models.Enumerations;
using AYN.Data.Repositories;
using AYN.Services.Data.Implementations;
using AYN.Services.Data.Interfaces;
using AYN.Services.Mapping;
using AYN.Web.ViewModels.Administration.Ads;
using AYN.Web.ViewModels.Ads;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace AYN.Services.Data.Tests
{
    [TestFixture]
    public class AdsServiceTests
    {
        private ApplicationDbContext dbContext;
        private EfDeletableEntityRepository<Ad> adsRepository;
        private EfDeletableEntityRepository<UserAdView> userAdViewsRepository;
        private Mock<ICloudinaryService> mockedICloudinaryService;
        private Mock<IFormFile> mockedIFormFile;
        private DbContextOptionsBuilder<ApplicationDbContext> options;
        private IAdsService adsService;

        [SetUp]
        public void SetUp()
        {
            this.options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.dbContext = new ApplicationDbContext(this.options.Options);
            this.adsRepository = new EfDeletableEntityRepository<Ad>(this.dbContext);
            this.userAdViewsRepository = new EfDeletableEntityRepository<UserAdView>(this.dbContext);
            this.mockedICloudinaryService = new Mock<ICloudinaryService>();
            this.mockedIFormFile = new Mock<IFormFile>();
            this.adsService = new AdsService(this.adsRepository, this.userAdViewsRepository, this.mockedICloudinaryService.Object);
            AutoMapperConfig.RegisterMappings(typeof(GetAdsViewModel).Assembly, typeof(Ad).Assembly);
            AutoMapperConfig.RegisterMappings(typeof(GetAllAdsViewModel).Assembly, typeof(Ad).Assembly);
        }

        [Test]
        public void GetCount_ShouldReturnCorrectResult()
        {
            var repository1 = new Mock<IDeletableEntityRepository<Ad>>();
            var repository2 = new Mock<IDeletableEntityRepository<UserAdView>>();
            var cld = new Mock<ICloudinaryService>();

            repository1.Setup(r => r.All()).Returns(new List<Ad>
            {
                new Ad(),
                new Ad(),
                new Ad(),
            }.AsQueryable());

            var service1 = new AdsService(repository1.Object, repository2.Object, cld.Object);

            Assert.AreEqual(3, service1.GetCount());

            repository1.Verify(x => x.All(), Times.Once);
        }

        [Test]
        public async Task CreateAd_ShouldBeCreateCorrect()
        {
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
                Pictures = new List<IFormFile>
                {
                    this.mockedIFormFile.Object,
                },
            };

            // ----------------------
            await this.adsService.CreateAsync(ad, "user.Id");
            Assert.AreEqual(1, this.adsRepository.All().Count());
        }

        [Test]
        public async Task EditAd_ShouldEditCorrect()
        {
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
                Pictures = new List<IFormFile>
                {
                    this.mockedIFormFile.Object,
                },
            };

            await this.adsService.CreateAsync(ad, "user.Id");

            var editedAd = new EditAdInputModel()
            {
                AdType = AdType.Private,
                AddressId = 2,
                CategoryId = 2,
                DeliveryTake = DeliveryTake.Seller,
                Description = "Awesome edited product!",
                Name = "Shlqpki 2",
                Price = 13.90M,
                ProductCondition = ProductCondition.Used,
                SubCategoryId = 2,
                Weight = 2,
                TownId = 2,
                Id = dbContext.Ads.FirstOrDefault(a => a.Name == "Shlqpki")?.Id,
                //Pictures = new List<IFormFile>
                //{
                //    moqIFormFile.Object,
                //},
            };

            await this.adsService.EditAsync(editedAd);

            Assert.AreNotEqual(ad.AdType, editedAd.AdType);
            Assert.AreNotEqual(ad.AddressId, editedAd.AddressId);
            Assert.AreNotEqual(ad.CategoryId, editedAd.CategoryId);
            Assert.AreNotEqual(ad.DeliveryTake, editedAd.DeliveryTake);
            Assert.AreNotEqual(ad.Description, editedAd.Description);
            Assert.AreNotEqual(ad.Name, editedAd.Name);
            Assert.AreNotEqual(ad.Price, editedAd.Price);
            Assert.AreNotEqual(ad.ProductCondition, editedAd.ProductCondition);
            Assert.AreNotEqual(ad.SubCategoryId, editedAd.SubCategoryId);
            Assert.AreNotEqual(ad.Weight, editedAd.Weight);
            Assert.AreNotEqual(ad.TownId, editedAd.TownId);
        }

        [Test]
        public async Task GetRecent12Ads_ShouldReturnThemCorrectly()
        {
            await this.FillUpAds(1, 15);

            var recentAds = await this.adsService.GetRecent12AdsAsync<GetAdsViewModel>();
            var recentAdsAsArray = recentAds as GetAdsViewModel[] ?? recentAds.ToArray();

            var j = 0;
            for (var i = 15; i >= 4; i--)
            {
                Assert.AreEqual($"Shlqpki{i}", recentAdsAsArray[j].Name);
                j++;
            }

            Assert.AreEqual(12, recentAdsAsArray.Count());
        }

        [Test]
        public async Task GetRecent12PromotedAds_ShouldReturnThemCorrectly()
        {
            await this.FillUpAds(1, 30);

            var allAds = this.adsRepository.All().ToList();

            for (var i = 0; i < allAds.Count(); i++)
            {
                if (i % 2 == 0)
                {
                    await this.adsService.Promote(DateTime.UtcNow.AddDays(1), allAds[i].Id);
                }
            }

            var promotedAds = await this.adsService.GetRecent12PromotedAdsAsync<GetAdsViewModel>();
            var promotedAdsAsArray = promotedAds as GetAdsViewModel[] ?? promotedAds.ToArray();

            var j = 0;
            for (var i = 29; i >= 7; i -= 2)
            {
                Assert.AreEqual($"Shlqpki{i}", promotedAdsAsArray[j].Name);
                j++;
            }

            Assert.AreEqual(12, promotedAdsAsArray.Count());
        }

        [Test]
        public async Task GetAll_WithoutParameters_ShouldReturnThemCorrectly()
        {
            await this.FillUpAds(1, 10);
            var allAds = await this.adsService.GetAllAsync<GetAdsViewModel>(null, "createdOnDesc", null);
            Assert.AreEqual(10, allAds.Count());
        }

        private async Task FillUpAds(int start, int end)
        {
            for (var i = start; i <= end; i++)
            {
                var userId = $"userId{i}";
                await this.adsService.CreateAsync(
                    new CreateAdInputModel()
                    {
                        AdType = AdType.Business,
                        AddressId = i,
                        CategoryId = i,
                        DeliveryTake = DeliveryTake.Buyer,
                        Description = $"Awesome product{i}!",
                        Name = $"Shlqpki{i}",
                        Price = i,
                        ProductCondition = ProductCondition.New,
                        SubCategoryId = i,
                        Weight = i,
                        TownId = i,
                        Pictures = new List<IFormFile>
                        {
                            this.mockedIFormFile.Object,
                        },
                    },
                    userId);

                Thread.Sleep(100);
            }
        }
    }
}
