using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Mapping;
using AYN.Web.ViewModels.Ads;

namespace AYN.Services.Data
{
    public class AdsService : IAdsService
    {
        private readonly IDeletableEntityRepository<Ad> adsRepository;
        private readonly IImageProcessingService imageProcessingService;
        private readonly IImageService imageService;

        public AdsService(
            IDeletableEntityRepository<Ad> adsRepository,
            IImageProcessingService imageProcessingService,
            IImageService imageService)
        {
            this.adsRepository = adsRepository;
            this.imageProcessingService = imageProcessingService;
            this.imageService = imageService;
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

            var index = 1;
            foreach (var picture in input.Pictures)
            {
                var extension = this.imageProcessingService.GetImageExtension(picture);
                this.imageProcessingService.ValidateImageExtension(extension);

                ad.Pictures.Add(new Picture() { Extension = extension });

                await this.imageService.CreateAsync(ad.Id, extension);

                var fullPhysicalPath = physicalPath + $"{index++}-{ad.Id}.{extension}";

                await using var fileStream = new FileStream(fullPhysicalPath, FileMode.Create);

                await picture.CopyToAsync(fileStream);
                await fileStream.DisposeAsync();

                await this.imageProcessingService.SaveImageLocallyAsync(fullPhysicalPath, 1000, 1000);
            }
        }

        public IEnumerable<T> GetRecent12Ads<T>()
            => this.adsRepository
                .All()
                .Take(12)
                .To<T>()
                .ToList();
    }
}
