using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Data.Interfaces;

namespace AYN.Services.Data.Implementations;

public class UserAdsViewsService : IUserAdsViewsService
{
    private readonly IDeletableEntityRepository<UserAdView> userAdsViewsRepository;

    public UserAdsViewsService(
        IDeletableEntityRepository<UserAdView> userAdsViewsRepository)
    {
        this.userAdsViewsRepository = userAdsViewsRepository;
    }

    public async Task CreateAsync(string userId, string adId)
    {
        if (this.userAdsViewsRepository.All().Any(uav => uav.UserId == userId && uav.AdId == adId))
        {
            return;
        }

        var userAdView = new UserAdView
        {
            AdId = adId,
            UserId = userId,
        };

        await this.userAdsViewsRepository.AddAsync(userAdView);
        await this.userAdsViewsRepository.SaveChangesAsync();
    }
}
