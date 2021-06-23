using System.Collections.Generic;
using System.Threading.Tasks;

using AYN.Data.Models;
using AYN.Web.ViewModels.Users;

namespace AYN.Services.Data
{
    public interface IUsersService
    {
        Task<T> GetProfileDetails<T>(string id);

        Task Follow(string followerId, string followeeId);

        Task Unfollow(string followerId, string followeeId);

        bool IsFollower(string followerId, string followeeId);

        bool IsUserExisting(string userId);

        Task GenerateDefaultAvatar(string firstName, string lastName, string userId, string wwwRootPath);

        Task GenerateDefaultThumbnail(string firstName, string lastName, string userId, string wwwRootPath);

        Task<T> GetByIdAsync<T>(string id);

        Task EditAsync(EditUserViewModel model, string wwwRootPath);

        Task<IEnumerable<T>> GetSuggestionPeople<T>(string userId, string openedUserId);
    }
}
