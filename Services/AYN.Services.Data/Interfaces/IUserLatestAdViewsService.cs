using System.Collections.Generic;
using System.Threading.Tasks;

namespace AYN.Services.Data.Interfaces
{
    public interface IUserLatestAdViewsService
    {
        Task<IEnumerable<T>> GetUserLatestAdViews<T>(string userId);
    }
}
