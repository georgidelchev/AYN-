using System.Collections.Generic;
using System.Threading.Tasks;

using AYN.Web.ViewModels.Ads;

namespace AYN.Services.Data
{
    public interface IAdsService
    {
        Task CreateAsync(CreateAdInputModel input, string userId, string imagePath);

        IEnumerable<T> GetRecent12Ads<T>();

        IEnumerable<T> GetAll<T>(int page, int itemsPerPage);

        int GetCount();
    }
}
