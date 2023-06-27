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

namespace AYN.Services.Data.Implementations;

public class AdsService : IAdsService
{
    private readonly IDeletableEntityRepository<Ad> adsRepository;
    private readonly ICloudinaryService cloudinaryService;

    public AdsService(
        IDeletableEntityRepository<Ad> adsRepository,
        ICloudinaryService cloudinaryService)
    {
        this.adsRepository = adsRepository;
        this.cloudinaryService = cloudinaryService;
    }

    public async Task CreateAsync(CreateAdInputModel input, string userId)
    {
        var imageUrls = new List<string>();
        foreach (var image in input.Pictures)
        {
            await using var ms = new MemoryStream();
            await image.CopyToAsync(ms);
            var destinationData = ms.ToArray();

            try
            {
                var imageUrl = await this.cloudinaryService.UploadPictureAsync(destinationData, image.FileName, "AdsImages", 900, 600);
                imageUrls.Add(imageUrl);
            }
            catch
            {
                // ignored
            }
        }

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
            AddressId = input.AddressId,
        };

        foreach (var imageUrl in imageUrls)
        {
            ad.Images.Add(new AdImage { ImageUrl = imageUrl });
        }

        await this.adsRepository.AddAsync(ad);
        await this.adsRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> GetRecent12AdsAsync<T>()
        => await this.adsRepository
            .All()
            .Where(a => !a.IsArchived)
            .Include(a => a.Images)
            .OrderBy(a => a.IsPromoted)
            .ThenByDescending(a => a.CreatedOn)
            .Take(12)
            .To<T>()
            .ToListAsync();

    public async Task<IEnumerable<T>> GetRecent12PromotedAdsAsync<T>()
        => await this.adsRepository
            .All()
            .Where(a => a.IsPromoted && !a.IsArchived)
            .Include(a => a.Images)
            .OrderByDescending(a => a.CreatedOn)
            .Take(12)
            .To<T>()
            .ToListAsync();

    public IEnumerable<T> GetAll<T>(string search = null, string town = null, string orderBy = null, int? categoryId = null, string letter = null)
    {
        var ads = this.adsRepository
            .All()
            .Include(a => a.Images)
            .Include(a => a.Town)
            .Include(a => a.UserAdViews)
            .Where(a => !a.IsArchived)
            .AsEnumerable();

        if (search is not null)
        {
            ads = ads.Where(this.GetAdPredicate(search));
        }

        if (categoryId is not null)
        {
            ads = ads.Where(this.GetAdPredicate(categoryId.Value.ToString()));
        }

        if (town is not null)
        {
            ads = ads.Where(this.GetAdPredicate(town));
        }

        if (letter is not null)
        {
            ads = ads.Where(a => a.Name.ToLower().Trim()[0].ToString() == letter.ToLower().Trim());
        }

        if (orderBy is not null)
        {
            ads = orderBy switch
            {
                "createdOnDesc" => ads
                    .OrderByDescending(a => a.IsPromoted)
                    .ThenByDescending(a => a.CreatedOn),
                "createdOnAsc" => ads
                    .OrderByDescending(a => a.IsPromoted)
                    .ThenBy(a => a.CreatedOn),
                "nameAsc" => ads
                    .OrderByDescending(a => a.IsPromoted)
                    .ThenBy(a => a.Name),
                "nameDesc" => ads
                    .OrderByDescending(a => a.IsPromoted)
                    .ThenByDescending(a => a.Name),
                "priceAsc" => ads
                    .OrderByDescending(a => a.IsPromoted)
                    .ThenBy(a => a.Price),
                "priceDesc" => ads
                    .OrderByDescending(a => a.IsPromoted)
                    .ThenByDescending(a => a.Price),
                "mostViewed" => ads
                    .OrderByDescending(a => a.UserAdViews.Count)
                    .ThenByDescending(a => a.Price),
                _ => throw new ArgumentException(),
            };
        }

        return ads.AsQueryable().To<T>().ToList();
    }

    public int GetCount()
        => this.adsRepository
            .All()
            .Count(a => !a.IsArchived);

    public async Task<T> GetDetails<T>(string id)
        => await this.adsRepository
            .All()
            .Where(a => a.Id == id && !a.IsArchived)
            .Include(a => a.Comments)
            .Include(a => a.UserAdViews)
            .Include(a => a.Images)
            .To<T>()
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<T>> GetUserAllAds<T>(string userId)
        => await this.adsRepository
            .All()
            .Where(u => u.AddedByUserId == userId && !u.IsArchived)
            .OrderByDescending(a => a.CreatedOn)
            .Include(a => a.AddedByUser)
            .Include(a => a.Images)
            .To<T>()
            .ToListAsync();

    public async Task<IEnumerable<T>> GetUserRecentAds<T>(string userId)
        => await this.adsRepository
            .All()
            .Where(u => u.AddedByUserId == userId && !u.IsArchived)
            .OrderByDescending(a => a.CreatedOn)
            .Take(12)
            .Include(a => a.AddedByUser)
            .Include(a => a.Images)
            .To<T>()
            .ToListAsync();

    public async Task<IEnumerable<T>> GetMoreFromUserAds<T>(string townName, int categoryId, int subCategoryId, string userId, string currentAdId)
        => await this.adsRepository
            .All()
            .Where(u => u.AddedByUserId == userId &&
                        !u.IsArchived &&
                        u.Town.Name == townName &&
                        u.CategoryId == categoryId &&
                        u.SubCategoryId == subCategoryId &&
                        u.Id != currentAdId)
            .OrderByDescending(a => a.CreatedOn)
            .Take(6)
            .Include(a => a.AddedByUser)
            .Include(a => a.Images)
            .To<T>()
            .ToListAsync();

    public async Task<IEnumerable<string>> GetAdsStartingLetters()
        => await this.adsRepository
            .All()
            .Select(a => a.Name.ToUpper()[0].ToString())
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

    public async Task<T> GetByIdAsync<T>(string adId)
    {
        var ad = await this.adsRepository
            .All()
            .Where(a => a.Id == adId)
            .To<T>()
            .FirstOrDefaultAsync();

        return ad;
    }

    public async Task EditAsync(EditAdInputModel input)
    {
        var ad = this.adsRepository
            .All()
            .FirstOrDefault(a => a.Id == input.Id);

        ad.Name = input.Name;
        ad.Description = input.Description;
        ad.Weight = input.Weight;
        ad.Price = input.Price;
        ad.TownId = input.TownId;
        ad.AddressId = input.AddressId;
        ad.CategoryId = input.CategoryId;
        ad.SubCategoryId = input.SubCategoryId;
        ad.ProductCondition = input.ProductCondition;
        ad.AdType = input.AdType;
        ad.ProductCondition = input.ProductCondition;
        ad.DeliveryTake = input.DeliveryTake;

        var imageUrls = new List<string>();
        foreach (var image in input.Images)
        {
            await using var ms = new MemoryStream();
            await image.CopyToAsync(ms);
            var destinationData = ms.ToArray();

            try
            {
                var imageUrl = await this.cloudinaryService.UploadPictureAsync(destinationData, image.FileName, "AdsImages", 900, 600);
                imageUrls.Add(imageUrl);
            }
            catch
            {
                // ignored
            }
        }

        foreach (var imageUrl in imageUrls.Where(i => i != null))
        {
            ad.Images.Add(new AdImage { ImageUrl = imageUrl });
        }

        this.adsRepository.Update(ad);
        await this.adsRepository.SaveChangesAsync();
    }

    public bool IsUserOwnsGivenAd(string userId, string adId)
        => this.adsRepository
            .All()
            .Any(a => a.AddedByUserId == userId && a.Id == adId);

    private Func<Ad, bool> GetAdPredicate(string term)
        => ad => this.FilterByProperty(ad, a => a.Name, term) ||
                 this.FilterByProperty(ad, a => a.Description, term) ||
                 this.FilterByProperty(ad, a => a.CategoryId.ToString(), term) ||
                 this.FilterByProperty(ad, a => a.SubCategoryId.ToString(), term) ||
                 this.FilterByProperty(ad, a => a.Town.Name, term);

    private bool FilterByProperty<T>(T ad, Func<T, string> extractor, string term)
        => extractor(ad)
            ?.ToLower()
            .Trim()
            .Contains(term.ToLower().Trim())
           ?? false;
}
