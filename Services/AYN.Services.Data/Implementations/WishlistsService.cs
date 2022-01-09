using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Data.Interfaces;
using AYN.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace AYN.Services.Data.Implementations;

public class WishlistsService : IWishlistsService
{
    private readonly IDeletableEntityRepository<Wishlist> wishlistsRepository;

    public WishlistsService(
        IDeletableEntityRepository<Wishlist> wishlistsRepository)
    {
        this.wishlistsRepository = wishlistsRepository;
    }

    public async Task AddAsync(string adId, string userId)
    {
        if (this.wishlistsRepository.All().Any(wl => wl.UserId == userId && wl.AdId == adId))
        {
            return;
        }

        var wishlist = new Wishlist
        {
            AdId = adId,
            UserId = userId,
        };

        await this.wishlistsRepository.AddAsync(wishlist);
        await this.wishlistsRepository.SaveChangesAsync();
    }

    public async Task RemoveAsync(string adId, string userId)
    {
        var wishlist = this.wishlistsRepository
            .All()
            .FirstOrDefault(wl => wl.UserId == userId && wl.AdId == adId);

        this.wishlistsRepository.Delete(wishlist);
        await this.wishlistsRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> Wishlist<T>(string userId)
        => await this.wishlistsRepository
            .All()
            .Where(wl => wl.UserId == userId)
            .OrderByDescending(wl => wl.CreatedOn)
            .Include(wl => wl.Ad)
            .Select(wl => wl.Ad)
            .To<T>()
            .ToListAsync();

    public int Count(string userId)
        => this.wishlistsRepository
            .All()
            .Count(wl => wl.UserId == userId);

    public bool IsUserHaveGivenAdInHisWishlist(string adId, string userId)
        => this.wishlistsRepository
            .All()
            .Any(wl => wl.UserId == userId && wl.AdId == adId);

    public async Task DeleteAsync(string adId)
    {
        var items = this.wishlistsRepository
            .All()
            .Where(wl => wl.AdId == adId)
            .ToList();

        foreach (var item in items)
        {
            this.wishlistsRepository.Delete(item);
        }

        await this.wishlistsRepository.SaveChangesAsync();
    }
}
