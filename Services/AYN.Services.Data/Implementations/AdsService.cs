using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Data.Interfaces;
using AYN.Services.Mapping;
using AYN.Web.ViewModels.Ads;
using Microsoft.EntityFrameworkCore;

namespace AYN.Services.Data.Implementations
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
                ProductCondition = input.ProductCondition,
                DeliveryTake = input.DeliveryTake,
            };

            await this.adsRepository.AddAsync(ad);
            await this.adsRepository.SaveChangesAsync();

            var index = 1;
            foreach (var picture in input.Pictures)
            {
                var extension = this.imageProcessingService.GetImageExtension(picture);
                this.imageProcessingService.ValidateImageExtension(extension);

                await this.imageService.CreateAsync(ad.Id, extension);

                var fullPhysicalPath = physicalPath + $"{index++}-{ad.Id}.{extension}";

                await using var fileStream = new FileStream(fullPhysicalPath, FileMode.Create);

                await picture.CopyToAsync(fileStream);
                await fileStream.DisposeAsync();

                await this.imageProcessingService.SaveImageLocallyAsync(fullPhysicalPath, 900, 600);
            }
        }

        public async Task<IEnumerable<T>> GetRecent12AdsAsync<T>()
            => await this.adsRepository
                .All()
                .OrderByDescending(a => a.CreatedOn)
                .Take(12)
                .To<T>()
                .ToListAsync();

        public async Task<IEnumerable<T>> GetAllAsync<T>(string search, string orderBy, int? categoryId)
        {
            var ads = this.adsRepository.All();

            if (search is not null)
            {
                search = search.ToLower().Trim();

                ads = ads
                    .Where(a => a.Name.ToLower().Contains(search) ||
                                a.Description.ToLower().Contains(search));
            }

            if (categoryId is not null)
            {
                ads = ads.Where(a => a.CategoryId == categoryId.Value ||
                                     a.SubCategoryId == categoryId.Value);
            }

            // order parameter (desc or asc)(orderBy)
            ads = orderBy.Contains("Desc") ?
                ads.OrderByDescendingDynamic(a => $"a.{orderBy.Substring(0, orderBy.Length - 4)}") :
                ads.OrderByDynamic(a => $"a.{orderBy.Substring(0, orderBy.Length - 3)}");

            return await ads
                .To<T>()
                .ToListAsync();
        }

        public int GetCount()
            => this.adsRepository
                .All()
                .Count();

        public async Task<T> GetDetails<T>(string id)
            => await this.adsRepository
                .All()
                .Where(a => a.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<T>> GetUserAllAds<T>(string userId)
            => await this.adsRepository
                .All()
                .Where(u => u.AddedByUserId == userId)
                .OrderByDescending(a => a.CreatedOn)
                .Include(a => a.AddedByUser)
                .To<T>()
                .ToListAsync();

        public async Task<IEnumerable<T>> GetUserRecentAds<T>(string userId)
            => await this.adsRepository
                .All()
                .Where(u => u.AddedByUserId == userId)
                .OrderByDescending(a => a.CreatedOn)
                .Take(12)
                .Include(a => a.AddedByUser)
                .To<T>()
                .ToListAsync();

        public Tuple<int, int, int, int> GetCounts()
        {
            var totalAdsCount = this.adsRepository
                .AllWithDeleted()
                .Count();

            var activeAdsCount = this.adsRepository
                .All()
                .Count(a => !a.IsArchived && !a.IsDeleted);

            var bannedAdsCount = this.adsRepository
                .All()
                .Count(a => a.IsDeleted);

            var archivedAdsCount = this.adsRepository
                .All()
                .Count(a => a.IsArchived);

            return new Tuple<int, int, int, int>(totalAdsCount, activeAdsCount, bannedAdsCount, archivedAdsCount);
        }

        public async Task Archive(string adId)
        {
            throw new NotImplementedException();
        }

        public Task UnArchive(string adId)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(string adId)
        {
            throw new NotImplementedException();
        }

        public async Task UnDelete(string adId)
        {
            throw new NotImplementedException();
        }

        public async Task Promote(string adId)
        {
            throw new NotImplementedException();
        }

        public async Task UnPromote(string adId)
        {
            throw new NotImplementedException();
        }
    }
}
