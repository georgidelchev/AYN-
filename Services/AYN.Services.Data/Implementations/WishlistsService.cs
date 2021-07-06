﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Data.Interfaces;
using AYN.Services.Mapping;
using Microsoft.EntityFrameworkCore;

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

        public async Task AddAsync(string adId, string userId)
        {
            if (this.wishlistsRepository.All().Any(wl => wl.UserId == userId && wl.AdId == adId))
            {
                return;
            }

            var wishlist = new Wishlist()
            {
                AdId = adId,
                UserId = userId,
            };

            await this.wishlistsRepository.AddAsync(wishlist);
            await this.wishlistsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> Wishlist<T>(string userId)
        {
            var wishlist = await this.wishlistsRepository
                .All()
                .Where(wl => wl.UserId == userId)
                .Include(wl => wl.Ad)
                .Select(wl => wl.Ad)
                .To<T>()
                .ToListAsync();

            return wishlist;
        }

        public int Count(string userId)
            => this.wishlistsRepository
                .All()
                .Count(wl => wl.UserId == userId);
    }
}