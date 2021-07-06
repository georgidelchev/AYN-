using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AYN.Web.ViewModels.Administration.Users;
using AYN.Web.ViewModels.Users;

namespace AYN.Services.Data.Interfaces
{
    public interface IUsersService
    {
        Task<IEnumerable<T>> GetAll<T>();

        Task<T> GetProfileDetails<T>(string id);

        Task Follow(string followerId, string followeeId);

        Task Unfollow(string followerId, string followeeId);

        Task<T> GetFollowers<T>(string userId);

        Task<T> GetFollowings<T>(string userId);

        bool IsFollower(string followerId, string followeeId);

        bool IsUserExisting(string userId);

        Task GenerateDefaultAvatar(string firstName, string lastName, string userId, string wwwRootPath);

        Task GenerateDefaultThumbnail(string firstName, string lastName, string userId, string wwwRootPath);

        Task<T> GetByIdAsync<T>(string id);

        Task EditAsync(EditUserViewModel model, string wwwRootPath);

        Task<IEnumerable<T>> GetSuggestionPeople<T>(string userId, string openedUserId);

        Tuple<int, int, int> GetCounts();

        Task Ban(BanUserInputModel input, string userId);

        Task Unban(string userId);

        Task AddAdToWishlist(string adId, string userId);
    }
}
