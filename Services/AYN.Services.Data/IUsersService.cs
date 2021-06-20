using System.Threading.Tasks;

namespace AYN.Services.Data
{
    public interface IUsersService
    {
        T GetProfileDetails<T>(string id);

        Task Follow(string followerId, string followeeId);

        bool IsAlreadyFollower(string followerId, string followeeId);

        Task GenerateDefaultAvatar(string firstName, string lastName, string userId, string wwwRootPath);
    }
}
