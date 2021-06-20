using System.Threading.Tasks;

namespace AYN.Services.Data
{
    public interface IUsersService
    {
        T GetProfileDetails<T>(string id);

        Task Follow(string followerId, string followeeId);

        Task Unfollow(string followerId, string followeeId);

        bool IsFollower(string followerId, string followeeId);

        Task GenerateDefaultAvatar(string firstName, string lastName, string userId, string wwwRootPath);

        Task<T> GetByIdAsync<T>(string id);
    }
}
