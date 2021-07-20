using AYN.Data;
using AYN.Data.Models;
using AYN.Data.Repositories;
using AYN.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AYN.Services.Data.Implementations;
using AYN.Web.ViewModels.Administration.Categories;
using AYN.Web.ViewModels.SubCategories;
using Microsoft.AspNetCore.Http;

namespace AYN.Services.Data.Tests
{
    [TestFixture]
    public class CategoriesServiceTests
    {
        private ApplicationDbContext dbContext;
        private EfDeletableEntityRepository<Category> categoriesRepository;
        private Mock<ISubCategoriesService> mockedISubCategoriesService;
        private Mock<ICloudinaryService> mockedICloudinaryService;
        private DbContextOptionsBuilder<ApplicationDbContext> options;
        private Mock<IFormFile> mockedIFormFile;
        private ICategoriesService categoriesService;

        [SetUp]
        public void SetUp()
        {
            this.options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.dbContext = new ApplicationDbContext(this.options.Options);
            this.categoriesRepository = new EfDeletableEntityRepository<Category>(this.dbContext);
            this.mockedISubCategoriesService = new Mock<ISubCategoriesService>();
            this.mockedICloudinaryService = new Mock<ICloudinaryService>();
            this.mockedIFormFile = new Mock<IFormFile>();
            this.categoriesService = new CategoriesService(this.categoriesRepository, this.mockedISubCategoriesService.Object, this.mockedICloudinaryService.Object);
        }

        [Test]
        public async Task CreateAsync_WithoutSubCategories_ShouldCreateCategorySuccessfully()
        {
            var category = new CreateCategoryInputModel()
            {
                Name = "Category",
                Picture = this.mockedIFormFile.Object,
            };

            await this.categoriesService.CreateAsync(category);
            var categoriesCount = this.dbContext.Categories.Count();

            Assert.AreEqual(1, categoriesCount);
        }

        [Test]
        public async Task CreateAsync_WithSubCategories_ShouldCreateCategorySuccessfully()
        {
            var category = new CreateCategoryInputModel()
            {
                Name = "Category",
                Picture = this.mockedIFormFile.Object,
            };

            for (var i = 1; i <= 10; i++)
            {
                category.SubCategories.Add(new CreateSubCategoryInputModel()
                {
                    Name = $"SubCategory{i}",
                });
            }

            await this.categoriesService.CreateAsync(category);

            var categories = this.dbContext.Categories.ToList();
            var dbCategory = this.dbContext
                .Categories
                .FirstOrDefault(c => c.Name == "Category");


            Assert.AreEqual(1, categories.Count);
            Assert.AreEqual(10, dbCategory.SubCategories.Count);
        }

        [Test]
        public void CreateAsync_WithExistingSubCategoryName_ShouldThrowAnInvalidOperationException()
        {
            this.mockedISubCategoriesService
                .Setup(sc => sc.IsSubCategoryExisting(It.IsAny<string>())).Returns(true);

            var category = new CreateCategoryInputModel()
            {
                Name = "Category",
                Picture = this.mockedIFormFile.Object,
            };

            for (var i = 1; i <= 5; i++)
            {
                category.SubCategories.Add(new CreateSubCategoryInputModel()
                {
                    Name = "SubCategory",
                });
            }

            Assert.ThrowsAsync<InvalidOperationException>(async () => await this.categoriesService.CreateAsync(category));
        }
    }
}
