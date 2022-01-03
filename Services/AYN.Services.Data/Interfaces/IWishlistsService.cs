using System.Collections.Generic;
using System.Threading.Tasks;

namespace AYN.Services.Data.Interfaces;

public interface IWishlistsService
{
    Task AddAsync(string adId, string userId);

    Task RemoveAsync(string adId, string userId);

    Task<IEnumerable<T>> Wishlist<T>(string userId);

    int Count(string userId);

    bool IsUserHaveGivenAdInHisWishlist(string adId, string userId);

    Task DeleteAsync(string adId);
}
