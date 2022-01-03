using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Data.Interfaces;
using AYN.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace AYN.Services.Data.Implementations;

public class UserLatestAdViewsService : IUserLatestAdViewsService
{
    private readonly IDeletableEntityRepository<UserAdView> userAdViewsRepository;

    public UserLatestAdViewsService(
        IDeletableEntityRepository<UserAdView> userAdViewsRepository)
    {
        this.userAdViewsRepository = userAdViewsRepository;
    }

    public async Task<IEnumerable<T>> GetUserLatestAdViews<T>(string userId)
        => await this.userAdViewsRepository
            .All()
            .Where(uav => uav.UserId == userId)
            .OrderByDescending(uav => uav.CreatedOn)
            .Take(12)
            .Select(uav => uav.Ad)
            .To<T>()
            .ToListAsync();
}
