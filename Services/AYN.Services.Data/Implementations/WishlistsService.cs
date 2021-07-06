using System.Linq;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Data.Interfaces;

namespace AYN.Services.Data.Implementations
{
    public class WishlistsService : IWishlistsService
    {
        private readonly IDeletableEntityRepository<Wishlist> wishlistsRepository;

        public WishlistsService(
            IDeletableEntityRepository<Wishlist> wishlistsRepository)
        {
            this.wishlistsRepository = wishlistsRepository;
        }

        public int Count(string userId)
            => this.wishlistsRepository
                .All()
                .Count(wl => wl.UserId == userId);
    }
}
