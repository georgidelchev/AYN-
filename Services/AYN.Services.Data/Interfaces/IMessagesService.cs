using System.Collections.Generic;
using System.Threading.Tasks;

namespace AYN.Services.Data.Interfaces
{
    public interface IMessagesService
    {
        Task CreateAsync(string content, string senderId, string receiverId);

        Task<string> GetLastActivityAsync(string currentUserId, string userId);

        Task<string> GetLastMessageAsync(string currentUserId, string userId);

        Task<IEnumerable<T>> GetAllWithUserAsync<T>(string currentUserId, string userId);

        Task<IEnumerable<T>> GetAllAsync<T>(string currentUserId);

        bool IsAllMessagesRead(string currentUserId, string userId);

        int GetUnreadMessagesCount(string userId);
    }
}
