using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Data.Models.Enumerations;
using AYN.Services.Data.Interfaces;
using AYN.Services.Mapping;
using AYN.Web.ViewModels.Ads;
using Microsoft.EntityFrameworkCore;

namespace AYN.Services.Data.Implementations
{
    public class AdsService : IAdsService
    {
        private readonly IDeletableEntityRepository<Ad> adsRepository;
        private readonly IImageService imageService;
        private readonly ICategoriesService categoriesService;
        private readonly ITownsService townsService;

        public AdsService(
            IDeletableEntityRepository<Ad> adsRepository,
            IImageService imageService,
            ICategoriesService categoriesService,
            ITownsService townsService)
        {
            this.adsRepository = adsRepository;
            this.imageService = imageService;
            this.categoriesService = categoriesService;
            this.townsService = townsService;
        }

        public async Task CreateAsync(CreateAdInputModel input, string userId, string imagePath)
        {
            var physicalPath = $"{imagePath}/img/AdsPictures/";
            Directory.CreateDirectory(physicalPath);

            var ad = new Ad
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

            await this.SaveImagesLocally(input, ad, physicalPath);
        }

        public async Task<IEnumerable<T>> GetRecent12AdsAsync<T>()
            => await this.adsRepository
                .All()
                .Where(a => !a.IsArchived)
                .OrderBy(a => a.IsPromoted)
                .ThenByDescending(a => a.CreatedOn)
                .Take(12)
                .To<T>()
                .ToListAsync();

        public async Task<IEnumerable<T>> GetRecent12PromotedAdsAsync<T>()
            => await this.adsRepository
                .All()
                .Where(a => a.IsPromoted && !a.IsArchived)
                .OrderByDescending(a => a.CreatedOn)
                .Take(12)
                .To<T>()
                .ToListAsync();

        public async Task<IEnumerable<T>> GetAllAsync<T>(string search, string orderBy, int? categoryId)
        {
            var ads = this.adsRepository.All().Where(a => !a.IsArchived);

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

            ads = orderBy switch
            {
                "createdOnDesc" => ads.OrderBy(a => a.IsPromoted).ThenByDescending(a => a.CreatedOn),
                "createdOnAsc" => ads.OrderBy(a => a.IsPromoted).ThenBy(a => a.CreatedOn),
                "nameAsc" => ads.OrderBy(a => a.IsPromoted).ThenBy(a => a.Name),
                "nameDesc" => ads.OrderBy(a => a.IsPromoted).ThenByDescending(a => a.Name),
                "priceAsc" => ads.OrderBy(a => a.IsPromoted).ThenBy(a => a.Price),
                "priceDesc" => ads.OrderBy(a => a.IsPromoted).ThenByDescending(a => a.Price),
                _ => throw new ArgumentException(),
            };

            return await ads.To<T>().ToListAsync();
        }

        public int GetCount()
            => this.adsRepository
                .All()
                .Count(a => !a.IsArchived);

        public async Task<T> GetDetails<T>(string id)
        {
            var a = await this.adsRepository
                .All()
                .Where(a => a.Id == id && !a.IsArchived)
                .Include(a => a.Comments)
                .Include(a => a.UserAdViews)
                .FirstOrDefaultAsync();

            return await this.adsRepository
                .All()
                .Where(a => a.Id == id && !a.IsArchived)
                .Include(a => a.Comments)
                .Include(a => a.UserAdViews)
                .To<T>()
                .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<T>> GetUserAllAds<T>(string userId)
            => await this.adsRepository
                .All()
                .Where(u => u.AddedByUserId == userId && !u.IsArchived)
                .OrderByDescending(a => a.CreatedOn)
                .Include(a => a.AddedByUser)
                .To<T>()
                .ToListAsync();

        public async Task<IEnumerable<T>> GetUserRecentAds<T>(string userId)
            => await this.adsRepository
                .All()
                .Where(u => u.AddedByUserId == userId && !u.IsArchived)
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

            var deletedAdsCount = this.adsRepository
                .AllWithDeleted()
                .Count(a => a.IsDeleted);

            var archivedAdsCount = this.adsRepository
                .All()
                .Count(a => a.IsArchived);

            return new Tuple<int, int, int, int>(totalAdsCount, activeAdsCount, deletedAdsCount, archivedAdsCount);
        }

        public async Task Archive(string adId)
        {
            var ad = this.adsRepository
                .All()
                .FirstOrDefault(a => a.Id == adId);

            ad.IsArchived = true;
            ad.ArchivedOn = DateTime.UtcNow;

            this.adsRepository.Update(ad);
            await this.adsRepository.SaveChangesAsync();
        }

        public async Task UnArchive(string adId)
        {
            var ad = this.adsRepository
                .All()
                .FirstOrDefault(a => a.Id == adId);


            ad.IsArchived = false;
            ad.ArchivedOn = null;

            this.adsRepository.Update(ad);
            await this.adsRepository.SaveChangesAsync();
        }

        public async Task Delete(string adId)
        {
            var ad = this.adsRepository
                .All()
                .FirstOrDefault(a => a.Id == adId);

            this.adsRepository.Delete(ad);
            await this.adsRepository.SaveChangesAsync();
        }

        public async Task Promote(DateTime promoteUntil, string adId)
        {
            var ad = this.adsRepository
                .All()
                .FirstOrDefault(a => a.Id == adId);

            ad.IsPromoted = true;
            ad.PromotedOn = DateTime.UtcNow;
            ad.PromotedUntil = promoteUntil;

            this.adsRepository.Update(ad);
            await this.adsRepository.SaveChangesAsync();
        }

        public async Task UnPromote(string adId)
        {
            var ad = this.adsRepository
                .All()
                .FirstOrDefault(a => a.Id == adId);

            ad.IsPromoted = false;
            ad.PromotedOn = null;
            ad.PromotedUntil = null;

            this.adsRepository.Update(ad);
            await this.adsRepository.SaveChangesAsync();
        }

        public bool IsAdExisting(string adId)
            => this.adsRepository
                .All()
                .Any(a => a.Id == adId);

        // Helper methods
        private async Task SaveImagesLocally(CreateAdInputModel input, Ad ad, string physicalPath)
        {
            var index = 1;
            foreach (var picture in input.Pictures)
            {
                var extension = this.imageService.GetImageExtension(picture);

                await this.imageService.CreateAsync(ad.Id, extension);

                var fullPhysicalPath = physicalPath + $"{index++}-{ad.Id}.{extension}";

                await using var fileStream = new FileStream(fullPhysicalPath, FileMode.Create);

                await picture.CopyToAsync(fileStream);
                await fileStream.DisposeAsync();

                await this.imageService.SaveImageLocallyAsync(fullPhysicalPath, 900, 600);
            }
        }
    }
}
