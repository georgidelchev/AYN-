using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Mapping;
using AYN.Web.ViewModels.Categories;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace AYN.Services.Data
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly IDeletableEntityRepository<SubCategory> subCategoriesRepository;
        private readonly string[] allowedImageExtensions = { "jpg", "png", "jfif", "exif", "gif", "bmp", "ppm", "pgm", "pbm", "pnm", "heif", "bat" };

        public CategoriesService(
            IDeletableEntityRepository<Category> categoriesRepository,
            IDeletableEntityRepository<SubCategory> subCategoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
            this.subCategoriesRepository = subCategoriesRepository;
        }

        public async Task CreateAsync(CreateCategoryInputModel input, string imagePath)
        {
            var physicalPath = $"{imagePath}/img/CategoriesPictures/";

            Directory.CreateDirectory($"{physicalPath}");

            var extension = Path
                .GetExtension(input.Picture.FileName)
                .TrimStart('.')
                .TrimEnd()
                .ToLower();

            if (!this.allowedImageExtensions.Contains(extension))
            {
                throw new InvalidOperationException("Invalid image.");
            }

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

            await SaveCategoryPictureLocally(fullPhysicalPath);
        }

        public IQueryable<T> GetAll<T>()
            => this.categoriesRepository
                .All()
                .To<T>();

        public async Task DeleteAsync(int id)
        {
            var category = this.categoriesRepository
                .All()
                .FirstOrDefault(c => c.Id == id);

            this.categoriesRepository.Delete(category);

            await this.categoriesRepository.SaveChangesAsync();
        }

        private static async Task SaveCategoryPictureLocally(string fullPhysicalPath)
        {
            using var categoryImage = await Image.LoadAsync(fullPhysicalPath);

            categoryImage.Mutate(ci => ci.Resize(100, 100));

            await categoryImage.SaveAsync($"{fullPhysicalPath}");
        }
    }
}
