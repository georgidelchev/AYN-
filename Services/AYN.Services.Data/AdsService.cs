using System;
using System.IO;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Web.ViewModels.Ads;

namespace AYN.Services.Data
{
    public class AdsService : IAdsService
    {
        private readonly IDeletableEntityRepository<Ad> adsRepository;
        private readonly IImageProcessingService imageProcessingService;

        public AdsService(
            IDeletableEntityRepository<Ad> adsRepository,
            IImageProcessingService imageProcessingService)
        {
            this.adsRepository = adsRepository;
            this.imageProcessingService = imageProcessingService;
        }

        public async Task CreateAsync(CreateAdInputModel input, string userId, string imagePath)
        {
            var physicalPath = $"{imagePath}/img/AdsPictures/";
            Directory.CreateDirectory(physicalPath);

            var ad = new Ad()
            {
                AdType = input.AdType,
                AddedByUserId = userId,
                CategoryId = input.CategoryId,
                Name = input.Name,
                Weight = input.Weight,
                TownId = input.TownId,
                SubCategoryId = input.SubCategoryId,
                Price = input.Price,
                Description = input.Description,
                IsPromoted = false,
            };

            await this.adsRepository.AddAsync(ad);
            await this.adsRepository.SaveChangesAsync();

            foreach (var picture in input.Pictures)
            {
                var extension = this.imageProcessingService.GetImageExtension(picture);
                this.imageProcessingService.ValidateImageExtension(extension);

                ad.Pictures.Add(new Picture() { Extension = extension });

                var fullPhysicalPath = physicalPath + $"{ad.Id}-{Guid.NewGuid()}.{extension}";

                await using var fileStream = new FileStream(fullPhysicalPath, FileMode.Create);

                await picture.CopyToAsync(fileStream);
                await fileStream.DisposeAsync();

                await this.imageProcessingService.SaveImageLocallyAsync(fullPhysicalPath, 1000, 1000);
            }
        }
    }
}
