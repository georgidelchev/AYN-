using System.Threading.Tasks;

namespace AYN.Services.Data.Interfaces
{
    public interface IUserAdsViewsService
    {
        Task CreateAsync(string userId, string adId);
    }
}
