using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AYN.Web.ViewModels.Ads;

namespace AYN.Services.Data.Interfaces;

public interface IAdsService
{
    Task CreateAsync(CreateAdInputModel input, string userId);

    Task<IEnumerable<T>> GetRecent12AdsAsync<T>();

    Task<IEnumerable<T>> GetRecent12PromotedAdsAsync<T>();

    IEnumerable<T> GetAll<T>(string search = null, string town = null, string orderBy = null, int? categoryId = null, string letter = null);

    int GetCount();

    Task<T> GetDetails<T>(string id);

    Task<IEnumerable<T>> GetUserAllAds<T>(string userId);

    Task<IEnumerable<T>> GetUserRecentAds<T>(string userId);

    Task<IEnumerable<T>> GetMoreFromUserAds<T>(string townName, int categoryId, int subCategoryId, string userId, string currentAdId);

    Task<IEnumerable<string>> GetAdsStartingLetters();

    Tuple<int, int, int, int> GetCounts();

    Task Archive(string adId);

    Task UnArchive(string adId);

    Task Delete(string adId);

    Task Promote(DateTime promoteUntil, string adId);

    Task UnPromote(string adId);

    bool IsAdExisting(string adId);

    Task<T> GetByIdAsync<T>(string adId);

    Task EditAsync(EditAdInputModel input);

    bool IsUserOwnsGivenAd(string userId, string adId);
}
