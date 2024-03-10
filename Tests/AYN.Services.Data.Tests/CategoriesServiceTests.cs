using System;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data;
using AYN.Data.Models;
using AYN.Data.Repositories;
using AYN.Services.Data.Implementations;
using AYN.Services.Data.Interfaces;
using AYN.Services.Mapping;
using AYN.Web.ViewModels.Administration.Categories;
using AYN.Web.ViewModels.SubCategories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace AYN.Services.Data.Tests;

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

        AutoMapperConfig.RegisterMappings(typeof(GetAllCategoriesViewModel).Assembly, typeof(GetAllCategoriesViewModel).Assembly);
    }

    [Test]
    public async Task CreateAsync_WithoutSubCategories_ShouldCreateCategorySuccessfully()
    {
        var category = new CreateCategoryInputModel
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
        var category = new CreateCategoryInputModel
        {
            Name = "Category",
            Picture = this.mockedIFormFile.Object,
        };

        for (var i = 1; i <= 10; i++)
        {
            category.SubCategories.Add(new CreateSubCategoryInputModel
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

        var category = new CreateCategoryInputModel
        {
            Name = "Category",
            Picture = this.mockedIFormFile.Object,
        };

        for (var i = 1; i <= 5; i++)
        {
            category.SubCategories.Add(new CreateSubCategoryInputModel
            {
                Name = "SubCategory",
            });
        }

        Assert.ThrowsAsync<InvalidOperationException>(async () => await this.categoriesService.CreateAsync(category));
    }

    [Test]
    public async Task GetAll_ShouldReturnAllCategoriesSuccessfully()
    {
        await this.FillUpCategories(10);
        var categories = this.categoriesService.GetAll<GetAllCategoriesViewModel>().ToList();
        Assert.AreEqual(10, categories.Count());
    }

    [Test]
    public async Task GetAll_WithDeleted_ShouldReturnOnlyNonDeletedCategoriesSuccessfully()
    {
        await this.FillUpCategories(10);

        var categoryId = this.dbContext
            .Categories
            .FirstOrDefault(c => c.Name == "Category2")
            ?.Id;

        await this.categoriesService.DeleteAsync(categoryId.Value);
        var categories = this.categoriesService.GetAll<GetAllCategoriesViewModel>().ToList();

        Assert.AreEqual(9, categories.Count);
    }

    [Test]
    public async Task GetAllWithDeleted_ShouldReturnAllWithDeletedCategoriesSuccessfully()
    {
        await this.FillUpCategories(10);

        var categoryId = this.dbContext
            .Categories
            .FirstOrDefault(c => c.Name == "Category2")
            ?.Id;

        await this.categoriesService.DeleteAsync(categoryId.Value);
        var categories = await this.categoriesService.GetAllWithDeletedAsync<GetAllCategoriesViewModel>();

        Assert.AreEqual(10, categories.Count());
    }

    [Test]
    public async Task GetAllAsKeyValuePairsAsync_ShouldReturnAllCategoriesAsKeyValuePairs()
    {
        await this.FillUpCategories(10);

        var categoriesAsKvp = await this.categoriesService.GetAllAsKeyValuePairsAsync();

        foreach (var (key, value) in categoriesAsKvp)
        {
            var categoryId = value.Substring(8);

            Assert.AreEqual(categoryId, key);
            Assert.AreEqual($"Category{categoryId}", value);
        }

        Assert.AreEqual(10, categoriesAsKvp.Count());
    }

    [Test]
    public async Task Delete_ShouldDeleteCategorySuccessfully()
    {
        await this.FillUpCategories(10);
        await this.categoriesService.DeleteAsync(5);

        var categoriesCount = this.dbContext.Categories.Count();

        Assert.AreEqual(9, categoriesCount);
    }

    [Test]
    public async Task UnDelete_ShouldUnDeleteCategorySuccessfully()
    {
        await this.FillUpCategories(10);
        await this.categoriesService.DeleteAsync(5);
        await this.categoriesService.UnDeleteAsync(5);

        var categoriesCount = this.dbContext.Categories.Count();

        Assert.AreEqual(10, categoriesCount);
    }

    [Test]
    public async Task GetTotalCount_ShouldReturnTotalCountOfTheCategoriesEvenWithDeleted()
    {
        await this.FillUpCategories(10);

        await this.categoriesService.DeleteAsync(1);
        await this.categoriesService.DeleteAsync(2);
        await this.categoriesService.DeleteAsync(3);

        Assert.AreEqual(10, this.categoriesService.GetTotalCount());
    }

    [Test]
    public async Task IsExisting_ShouldReturnTrueForExistingCategoryId()
    {
        await this.FillUpCategories(10);
        var isExisting = this.categoriesService.IsExisting(5);
        Assert.IsTrue(isExisting);
    }

    [Test]
    public async Task IsExisting_ShouldReturnFalseForNonExistingCategoryId()
    {
        await this.FillUpCategories(10);
        var isExisting = this.categoriesService.IsExisting(11);
        Assert.IsFalse(isExisting);
    }

    [Test]
    public async Task GetCounts_ShouldReturnCorrectAdCounts()
    {
        await this.FillUpCategories(5);

        await this.categoriesService.DeleteAsync(1);
        await this.categoriesService.DeleteAsync(2);

        await this.dbContext.SaveChangesAsync();

        var counts = this.categoriesService.GetCounts();

        Assert.AreEqual(3, counts.Item1);
        Assert.AreEqual(2, counts.Item2);
    }

    private async Task FillUpCategories(int categoriesCount)
    {
        for (var i = 1; i <= categoriesCount; i++)
        {
            await this.categoriesService
                .CreateAsync(new CreateCategoryInputModel
                {
                    Name = $"Category{i}",
                    Picture = this.mockedIFormFile.Object,
                });
        }
    }
}
