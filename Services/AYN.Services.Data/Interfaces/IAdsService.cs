using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AYN.Web.ViewModels.Ads;

namespace AYN.Services.Data.Interfaces
{
    public interface IAdsService
    {
        Task CreateAsync(CreateAdInputModel input, string userId, string imagePath);

        Task<IEnumerable<T>> GetRecent12AdsAsync<T>();

        Task<IEnumerable<T>> GetRecent12PromotedAdsAsync<T>();

        Task<IEnumerable<T>> GetAllAsync<T>(string search, string orderBy, int? categoryId);

        int GetCount();

        Task<T> GetDetails<T>(string id);

        Task<IEnumerable<T>> GetUserAllAds<T>(string userId);

        Task<IEnumerable<T>> GetUserRecentAds<T>(string userId);

        Tuple<int, int, int, int> GetCounts();

        Task Archive(string adId);

        Task UnArchive(string adId);

        Task Delete(string adId);

        Task Promote(DateTime promoteUntil, string adId);

        Task UnPromote(string adId);
    }
}
