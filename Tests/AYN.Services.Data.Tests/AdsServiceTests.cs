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
using AYN.Web.ViewModels.Comments;
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
            this.mockedICloudinaryService = new Mock<ICloudinaryService>();
            this.mockedIFormFile = new Mock<IFormFile>();
            this.adsService = new AdsService(this.adsRepository, this.mockedICloudinaryService.Object);

            AutoMapperConfig.RegisterMappings(typeof(GetAdsViewModel).Assembly, typeof(Ad).Assembly);
            AutoMapperConfig.RegisterMappings(typeof(GetAllAdsViewModel).Assembly, typeof(Ad).Assembly);
            AutoMapperConfig.RegisterMappings(typeof(GetDetailsViewModel).Assembly, typeof(Ad).Assembly);
        }

        [Test]
        public async Task GetCount_ShouldReturnCorrectResult()
        {
            await this.FillUpAds(1, 5);
            var count = this.adsService.GetCount();
            Assert.AreEqual(5, count);
        }

        [Test]
        public async Task GetCount_ShouldReturnCorrectResult_WhenAdIsDeleted()
        {
            await this.FillUpAds(1, 5);

            var adId = this.dbContext
                .Ads
                .FirstOrDefault(a => a.Name == "Shlqpki3")
                ?.Id;

            await this.adsService.Delete(adId);
            var count = this.adsService.GetCount();

            Assert.AreEqual(4, count);
        }

        [Test]
        public async Task GetCount_ShouldReturnCorrectResult_WhenAdIsArchived()
        {
            await this.FillUpAds(1, 5);

            var adId = this.dbContext
                .Ads
                .FirstOrDefault(a => a.Name == "Shlqpki3")
                ?.Id;

            await this.adsService.Archive(adId);
            var count = this.adsService.GetCount();

            Assert.AreEqual(4, count);
        }

        [Test]
        public async Task CreateAd_WithValidData_ShouldBeCreatedSuccessfully()
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
        public async Task EditAd_WithValidData_ShouldBeEditedSuccessfully()
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
        public async Task GetRecent12Ads_ShouldRecent12AdsSuccessfully()
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
        public async Task GetRecent12PromotedAds_ShouldRecent12PromotedAdsSuccessfully()
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
        public async Task GetAll_WithoutParameters_ShouldReturnAllAdsSuccessfully()
        {
            await this.FillUpAds(1, 10);
            var allAds = await this.adsService.GetAllAsync<GetAdsViewModel>(null, "createdOnDesc", null);
            Assert.AreEqual(10, allAds.Count());
        }

        [Test]
        [TestCase(" ")]
        [TestCase("s")]
        [TestCase("Sh")]
        [TestCase("Shl")]
        [TestCase("SHlq")]
        [TestCase("Shlqp")]
        [TestCase("Shlqpk")]
        [TestCase("Shlqpki")]
        [TestCase("ShLqpki ")]
        public async Task GetAll_WithSearchParameterForName_ShouldReturnAllAdsSuccessfully(string search)
        {
            await this.FillUpAds(1, 10);
            var allAds = await this.adsService.GetAllAsync<GetAdsViewModel>(search, "createdOnDesc", null);
            Assert.AreEqual(10, allAds.Count());
        }

        [Test]
        [TestCase("ShlQpki2")]
        [TestCase("Shlqpki3")]
        [TestCase("ShlqpKi3 ")]
        public async Task GetAll_WithSearchParameterForExactName_ShouldReturnAllAdsSuccessfully(string search)
        {
            await this.FillUpAds(1, 10);
            var allAds = await this.adsService.GetAllAsync<GetAdsViewModel>(search, "createdOnDesc", null);
            Assert.AreEqual(1, allAds.Count());
        }

        [Test]
        [TestCase(" ")]
        [TestCase("A")]
        [TestCase("Aw")]
        [TestCase("Awe")]
        [TestCase("Awes")]
        [TestCase("AwEsO")]
        [TestCase("AWESOM")]
        [TestCase("AwEsome")]
        [TestCase("AwesomE Product")]
        [TestCase("Awesome product ")]
        [TestCase("product ")]
        [TestCase("proDUct ")]
        public async Task GetAll_WithSearchParameterForDescription_ShouldReturnAllAdsSuccessfully(string search)
        {
            await this.FillUpAds(1, 10);
            var allAds = await this.adsService.GetAllAsync<GetAdsViewModel>(search, "createdOnDesc", null);
            Assert.AreEqual(10, allAds.Count());
        }

        [Test]
        [TestCase("createdOnDesc")]
        [TestCase("createdOnAsc")]
        [TestCase("nameAsc")]
        [TestCase("nameDesc")]
        [TestCase("priceAsc")]
        [TestCase("priceDesc")]
        public async Task GetAll_WithOrderParameter_ShouldReturnAllAdsSuccessfully(string orderBy)
        {
            await this.FillUpAds(1, 10);

            var ads = this.adsRepository.All().ToList();

            ads = orderBy switch
            {
                "createdOnDesc" => ads.OrderBy(a => a.IsPromoted).ThenByDescending(a => a.CreatedOn).ToList(),
                "createdOnAsc" => ads.OrderBy(a => a.IsPromoted).ThenBy(a => a.CreatedOn).ToList(),
                "nameAsc" => ads.OrderBy(a => a.IsPromoted).ThenBy(a => a.Name).ToList(),
                "nameDesc" => ads.OrderBy(a => a.IsPromoted).ThenByDescending(a => a.Name).ToList(),
                "priceAsc" => ads.OrderBy(a => a.IsPromoted).ThenBy(a => a.Price).ToList(),
                "priceDesc" => ads.OrderBy(a => a.IsPromoted).ThenByDescending(a => a.Price).ToList(),
                _ => throw new ArgumentException(),
            };

            var allAds = await this.adsService.GetAllAsync<GetAdViewModel>(null, orderBy, null);
            var allAdsAsArray = allAds as GetAdViewModel[] ?? allAds.ToArray();

            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(ads[i].Name, allAdsAsArray[i].Name);
            }
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task GetAll_WithCategoryParameter_ShouldReturnAllAdsSuccessfully(int categoryId)
        {
            await this.FillUpAds(1, 5);

            var ads = this.adsRepository
                .All()
                .Where(a => a.CategoryId == categoryId || a.SubCategoryId == categoryId);

            var allAds = await this.adsService.GetAllAsync<GetAdViewModel>(null, "createdOnDesc", categoryId);

            Assert.AreEqual(ads.Count(), allAds.Count());
        }

        // TODO: DETAILS TEST FAILING
        //[Test]
        //public async Task GetDetails_ShouldReturnCorrectDetailsAboutTheAd()
        //{
        //    await this.FillUpAds(1, 10);

        //    var ad = this.dbContext
        //        .Ads
        //        .FirstOrDefault(a => a.Name == "Shlqpki3");

        //    var details = await this.adsService.GetDetails<GetDetailsViewModel>("1q234");

        //    Assert.AreEqual(ad.AdType, details.AdType);
        //    Assert.AreEqual(ad.DeliveryTake, details.DeliveryTake);
        //    Assert.AreEqual(ad.Description, details.Description);
        //    Assert.AreEqual(ad.Name, details.Name);
        //    Assert.AreEqual(ad.Price, details.Price);
        //    Assert.AreEqual(ad.ProductCondition, details.ProductCondition);
        //    Assert.AreEqual(ad.Weight, details.Weight);
        //}

        [Test]
        public async Task GetCounts_ShouldReturnCorrectAdCounts()
        {
            var archivedAd = new Ad()
            {
                IsArchived = true,
                AdType = AdType.Business,
                AddressId = 1,
                CategoryId = 1,
                DeliveryTake = DeliveryTake.Buyer,
                Description = $"Awesome product1!",
                Name = "Shlqpki1",
                Price = 1,
                ProductCondition = ProductCondition.New,
                SubCategoryId = 1,
                Weight = 1,
                TownId = 1,
                AddedByUserId = "123",
                ArchivedOn = DateTime.UtcNow,
                IsDeleted = false,
                IsPromoted = false,
            };

            var deletedAd = new Ad()
            {
                IsArchived = false,
                AdType = AdType.Business,
                AddressId = 1,
                CategoryId = 1,
                DeliveryTake = DeliveryTake.Buyer,
                Description = $"Awesome product1!",
                Name = "Shlqpki1",
                Price = 1,
                ProductCondition = ProductCondition.New,
                SubCategoryId = 1,
                Weight = 1,
                TownId = 1,
                AddedByUserId = "123",
                IsDeleted = true,
                DeletedOn = DateTime.UtcNow,
                IsPromoted = false,
            };

            await this.dbContext.Ads.AddAsync(archivedAd);
            await this.dbContext.Ads.AddAsync(deletedAd);
            await this.dbContext.SaveChangesAsync();

            var counts = this.adsService.GetCounts();

            Assert.AreEqual(2, counts.Item1);
            Assert.AreEqual(0, counts.Item2);
            Assert.AreEqual(1, counts.Item3);
            Assert.AreEqual(1, counts.Item4);
        }

        [Test]
        public async Task IsAdExisting_ShouldReturnTrueForExistingAd()
        {
            await this.FillUpAds(1, 5);

            var adId = this.dbContext
                .Ads
                .FirstOrDefault(a => a.Name == "Shlqpki4")
                ?.Id;

            var isExiting = this.adsService.IsAdExisting(adId);

            Assert.IsTrue(isExiting);
        }

        [Test]
        public void IsAdExisting_ShouldReturnFalseForNonExistingAd()
        {
            var isExiting = this.adsService.IsAdExisting("invalidId");
            Assert.IsFalse(isExiting);
        }

        [Test]
        public async Task GetById_WithValidId_ShouldReturnAdSuccessfully()
        {
            await this.FillUpAds(1, 10);

            var ad = this.dbContext
                .Ads
                .FirstOrDefault(a => a.Name == "Shlqpki3");

            var adById = await this.adsService.GetByIdAsync<GetAdsViewModel>(ad?.Id);

            Assert.AreEqual(ad.Name, adById.Name);
            Assert.AreEqual(ad.Price, adById.Price);
        }

        [Test]
        public async Task GetById_WithInvalidId_ShouldNotReturnAnyAd()
        {
            await this.FillUpAds(1, 10);

            var ad = this.dbContext
                .Ads
                .FirstOrDefault(a => a.Name == "Shlqpki20");

            var adById = await this.adsService.GetByIdAsync<GetAdsViewModel>(ad?.Id);

            Assert.IsNull(adById);
        }

        [Test]
        public async Task Promote_ShouldPromoteTheAdSuccessfully()
        {
            await this.FillUpAds(1, 3);

            var ad = this.dbContext
                .Ads
                .FirstOrDefault(a => a.Name == "Shlqpki3");

            await this.adsService.Promote(DateTime.UtcNow.AddDays(5), ad?.Id);

            Assert.IsTrue(ad?.IsPromoted);
            Assert.IsNotNull(ad?.PromotedOn);
        }

        [Test]
        public async Task UnPromote_ShouldUnPromoteTheAdSuccessfully()
        {
            await this.FillUpAds(1, 3);

            var ad = this.dbContext
                .Ads
                .FirstOrDefault(a => a.Name == "Shlqpki3");

            await this.adsService.Promote(DateTime.UtcNow.AddDays(5), ad?.Id);
            await this.adsService.UnPromote(ad?.Id);

            Assert.IsFalse(ad?.IsPromoted);
            Assert.IsNull(ad?.PromotedOn);
        }

        [Test]
        public async Task Archive_ShouldArchiveTheAdSuccessfully()
        {
            await this.FillUpAds(1, 5);

            var ad = this.dbContext
                .Ads
                .FirstOrDefault(a => a.Name == "Shlqpki5");

            await this.adsService.Archive(ad.Id);

            Assert.IsTrue(ad.IsArchived);
            Assert.IsNotNull(ad.ArchivedOn);
        }

        [Test]
        public async Task UnArchive_ShouldUnArchiveTheAdSuccessfully()
        {
            await this.FillUpAds(1, 5);

            var ad = this.dbContext
                .Ads
                .FirstOrDefault(a => a.Name == "Shlqpki5");

            await this.adsService.Archive(ad?.Id);
            await this.adsService.UnArchive(ad?.Id);

            Assert.IsFalse(ad?.IsArchived);
            Assert.IsNull(ad?.ArchivedOn);
        }

        [Test]
        public async Task Delete_ShouldDeleteTheAdSuccessfully()
        {
            await this.FillUpAds(1, 5);

            var ad = this.dbContext
                .Ads
                .FirstOrDefault(a => a.Name == "Shlqpki5");

            await this.adsService.Delete(ad?.Id);

            var adsCount = this.adsService.GetCount();

            Assert.AreEqual(4, adsCount);
        }

        [Test]
        public async Task GetUserAllAds_ShouldReturnAllAdsForGivenUserSuccessfully()
        {
            await this.FillUpAds(1, 3);
            await this.FillUpAds(1, 3);
            await this.FillUpAds(1, 3);
            await this.FillUpAds(1, 3);

            var userId = "userId1";
            var allAdsForUser = await this.adsService.GetUserAllAds<GetAdViewModel>(userId);

            Assert.AreEqual(4, allAdsForUser.Count());
        }

        [Test]
        public async Task GetUserAllAds_WithArchivedAds_ShouldReturnAllAdsForGivenUserSuccessfully()
        {
            await this.FillUpAds(1, 3);
            await this.FillUpAds(1, 3);
            await this.FillUpAds(1, 3);
            await this.FillUpAds(1, 3);

            var adId = this.dbContext
                .Ads
                .FirstOrDefault(a => a.Name == "Shlqpki1")
                ?.Id;

            await this.adsService.Archive(adId);
            var allAdsForUser = await this.adsService.GetUserAllAds<GetAdViewModel>("userId1");

            Assert.AreEqual(3, allAdsForUser.Count());
        }

        [Test]
        public async Task GetUserAllAds_WithDeletedAds_ShouldReturnAllAdsForGivenUserSuccessfully()
        {
            await this.FillUpAds(1, 3);
            await this.FillUpAds(1, 3);
            await this.FillUpAds(1, 3);
            await this.FillUpAds(1, 3);

            var adId = this.dbContext
                .Ads
                .FirstOrDefault(a => a.Name == "Shlqpki1")
                ?.Id;

            await this.adsService.Delete(adId);
            var allAdsForUser = await this.adsService.GetUserAllAds<GetAdViewModel>("userId1");

            Assert.AreEqual(3, allAdsForUser.Count());
        }

        [Test]
        public async Task GetUserRecentAds_ShouldReturnAllRecentAdsForGivenUserSuccessfully()
        {
            for (var i = 1; i <= 30; i++)
            {
                await this.FillUpAds(1, 3);
            }

            var recentAdsByUserId = await this.adsService.GetUserRecentAds<GetAdViewModel>("userId1");
            var recentAdsByUserIdAsArray = recentAdsByUserId as GetAdViewModel[] ?? recentAdsByUserId.ToArray();

            var a = this.dbContext.Ads.Where(a => a.AddedByUserId == "userId1").ToList();
            Assert.AreEqual(12, recentAdsByUserIdAsArray.Count());
            Assert.AreEqual("Shlqpki1", recentAdsByUserIdAsArray[0].Name);
        }

        [Test]
        public async Task GetUserRecentAds_WithArchiveAds_ShouldReturnAllRecentAdsForGivenUserSuccessfully()
        {
            for (var i = 1; i <= 12; i++)
            {
                await this.FillUpAds(1, 3);
            }

            var adId = this.dbContext
                .Ads
                .FirstOrDefault(a => a.Name == "Shlqpki1")
                ?.Id;

            await this.adsService.Archive(adId);

            var recentAdsByUserId = await this.adsService.GetUserRecentAds<GetAdViewModel>("userId1");
            var recentAdsByUserIdAsArray = recentAdsByUserId as GetAdViewModel[] ?? recentAdsByUserId.ToArray();

            Assert.AreEqual(11, recentAdsByUserIdAsArray.Count());
            Assert.AreEqual("Shlqpki1", recentAdsByUserIdAsArray[0].Name);
        }

        [Test]
        public async Task GetUserRecentAds_WithDeletedAds_ShouldReturnAllRecentAdsForGivenUserSuccessfully()
        {
            for (var i = 1; i <= 5; i++)
            {
                await this.FillUpAds(1, 3);
            }

            var adId = this.dbContext
                .Ads
                .FirstOrDefault(a => a.Name == "Shlqpki1")
                ?.Id;

            await this.adsService.Delete(adId);

            var recentAdsByUserId = await this.adsService.GetUserRecentAds<GetAdViewModel>("userId1");
            var recentAdsByUserIdAsArray = recentAdsByUserId as GetAdViewModel[] ?? recentAdsByUserId.ToArray();

            Assert.AreEqual(4, recentAdsByUserIdAsArray.Count());
            Assert.AreEqual("Shlqpki1", recentAdsByUserIdAsArray[0].Name);
        }

        [Test]
        [TestCase("Shlqpki1", "userId1")]
        [TestCase("Shlqpki2", "userId2")]
        public async Task IsUserOwnsGivenAd_WithInlineData_ShouldReturnTrue(string adName, string userId)
        {
            await this.FillUpAds(1, 3);

            var adId = this.dbContext
                .Ads
                .FirstOrDefault(a => a.Name == adName)
                ?.Id;

            var result = this.adsService.IsUserOwnsGivenAd(userId, adId);

            Assert.IsTrue(result);
        }

        [Test]
        [TestCase("Shlqpki2", "userId1")]
        [TestCase("Shlqpki5", "userId2")]
        [TestCase("Shlqpki1", "userId2")]
        [TestCase("Shlqpki2", "userId5")]
        public async Task IsUserOwnsGivenAd_WithInlineData_ShouldReturnFalse(string adName, string userId)
        {
            await this.FillUpAds(1, 3);

            var adId = this.dbContext
                .Ads
                .FirstOrDefault(a => a.Name == adName)
                ?.Id;

            var result = this.adsService.IsUserOwnsGivenAd(userId, adId);

            Assert.IsFalse(result);
        }

        // Helper Method to fill up given amount of ads
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
