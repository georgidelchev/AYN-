using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Data.Interfaces;
using AYN.Services.Mapping;
using AYN.Web.ViewModels.Administration.Categories;
using AYN.Web.ViewModels.Categories;
using Microsoft.EntityFrameworkCore;
using EditCategoryInputModel = AYN.Web.ViewModels.Administration.Categories.EditCategoryInputModel;

namespace AYN.Services.Data.Implementations
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly IDeletableEntityRepository<SubCategory> subCategoriesRepository;
        private readonly ISubCategoriesService subCategoriesService;
        private readonly IImageProcessingService imageProcessingService;

        public CategoriesService(
            IDeletableEntityRepository<Category> categoriesRepository,
            IDeletableEntityRepository<SubCategory> subCategoriesRepository,
            ISubCategoriesService subCategoriesService,
            IImageProcessingService imageProcessingService)
        {
            this.categoriesRepository = categoriesRepository;
            this.subCategoriesRepository = subCategoriesRepository;
            this.subCategoriesService = subCategoriesService;
            this.imageProcessingService = imageProcessingService;
        }

        public async Task CreateAsync(CreateCategoryInputModel input, string imagePath)
        {
            var physicalPath = $"{imagePath}/img/CategoriesPictures/";
            Directory.CreateDirectory($"{physicalPath}");

            var extension = this.imageProcessingService.GetImageExtension(input.Picture);
            this.imageProcessingService.IsExtensionValid(extension);

            var category = new Category()
            {
                Name = input.Name,
                PictureExtension = extension,
            };

            if (input.SubCategories.Any())
            {
                foreach (var subCategory in input.SubCategories)
                {
                    var currentSubCategory = this.subCategoriesRepository
                        .All()
                        .FirstOrDefault(sc => sc.CategoryId == category.Id)
                    ?? new SubCategory()
                    {
                        Name = subCategory.Name,
                    };

                    if (this.subCategoriesService.IsSubCategoryExisting(currentSubCategory.Name))
                    {
                        throw new InvalidOperationException($"This subCategory '{currentSubCategory.Name}' is already taken by another category!");
                    }

                    category.SubCategories
                        .Add(currentSubCategory);
                }
            }

            await this.categoriesRepository.AddAsync(category);
            await this.categoriesRepository.SaveChangesAsync();

            var fullPhysicalPath = physicalPath + $"{category.Id}.{extension}";

            await using var fileStream = new FileStream(fullPhysicalPath, FileMode.Create);

            await input.Picture.CopyToAsync(fileStream);
            await fileStream.DisposeAsync();

            await this.imageProcessingService.SaveImageLocallyAsync(fullPhysicalPath, 100, 100);
        }

        public IQueryable<T> GetAll<T>()
            => this.categoriesRepository
                .All()
                .To<T>();

        public async Task<IEnumerable<T>> GetAllWithDeletedAsync<T>()
            => await this.categoriesRepository
                .AllWithDeleted()
                .To<T>()
                .ToListAsync();

        public async Task<IEnumerable<KeyValuePair<string, string>>> GetAllAsKeyValuePairsAsync()
            => await this.categoriesRepository
                .All()
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                })
                .OrderBy(c => c.Name)
                .Select(c => new KeyValuePair<string, string>(c.Id.ToString(), c.Name))
                .ToListAsync();

        public async Task AddSubCategoryAsync(AddSubCategoryInputModel input, int categoryId)
            => await this.subCategoriesService.CreateAsync(input, categoryId);

        public async Task<T> GetByIdAsync<T>(int categoryId)
            => await this.categoriesRepository
                .All()
                .Where(c => c.Id == categoryId)
                .To<T>()
                .FirstOrDefaultAsync();

        public async Task UpdateAsync(EditCategoryInputModel input, int categoryId, string imagePath)
        {
            var category = this.categoriesRepository
                .All()
                .FirstOrDefault(c => c.Id == categoryId);

            category.Name = input.Name;

            this.categoriesRepository.Update(category);
            await this.categoriesRepository.SaveChangesAsync();

            if (input.Picture != null)
            {
                var physicalPath = $"{imagePath}/img/CategoriesPictures/";

                Directory.CreateDirectory($"{physicalPath}");
                File.Delete(physicalPath + $"{category.Id}.{category.PictureExtension}");

                var extension = this.imageProcessingService.GetImageExtension(input.Picture);
                this.imageProcessingService.IsExtensionValid(extension);

                category.PictureExtension = extension;

                this.categoriesRepository.Update(category);
                await this.categoriesRepository.SaveChangesAsync();

                var fullPhysicalPath = physicalPath + $"{category.Id}.{extension}";

                await using var fileStream = new FileStream(fullPhysicalPath, FileMode.Create);

                await input.Picture.CopyToAsync(fileStream);
                await fileStream.DisposeAsync();

                await this.imageProcessingService.SaveImageLocallyAsync(fullPhysicalPath, 100, 100);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var category = this.categoriesRepository
                .All()
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                throw new InvalidOperationException($"The category doesn't exist!");
            }

            this.categoriesRepository.Delete(category);

            await this.categoriesRepository.SaveChangesAsync();
        }

        public async Task UnDeleteAsync(int id)
        {
            var category = this.categoriesRepository
                .AllWithDeleted()
                .FirstOrDefault(c => c.Id == id);

            category.IsDeleted = false;
            category.DeletedOn = null;

            this.categoriesRepository.Update(category);
            await this.categoriesRepository.SaveChangesAsync();
        }

        public Tuple<int, int> GetCounts()
        {
            var activeCategories = this.categoriesRepository
                .All()
                .Count(c => !c.IsDeleted);

            var deletedCategories = this.categoriesRepository
                .AllWithDeleted()
                .Count(c => c.IsDeleted);

            return new Tuple<int, int>(activeCategories, deletedCategories);
        }

        public int GetTotalCount()
            => this.categoriesRepository
                .AllWithDeleted()
                .Count();

        public bool IsExisting(int categoryId)
            => this.categoriesRepository
                .All()
                .Any(c => c.Id == categoryId);

        public bool IsCategoryContainsGivenSubCategory(int categoryId, int subCategoryId)
        {
            var category = this.categoriesRepository
                .All()
                .Include(c => c.SubCategories)
                .FirstOrDefault(c => c.Id == categoryId);

            return category.SubCategories.Any(sc => sc.Id == subCategoryId);
        }
    }
}
