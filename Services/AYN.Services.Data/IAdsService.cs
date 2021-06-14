using System.Collections.Generic;
using System.Threading.Tasks;

using AYN.Web.ViewModels.Ads;

namespace AYN.Services.Data
{
    public interface IAdsService
    {
        Task CreateAsync(CreateAdInputModel input, string userId, string imagePath);

        Task<IEnumerable<T>> GetRecent12AdsAsync<T>();

        Task<IEnumerable<T>> GetAllAsync<T>(int page, int itemsPerPage);

        int GetCount();

        Task<IEnumerable<T>> GetFromSearchAsync<T>(string search, string orderBy, string town);
    }
}
