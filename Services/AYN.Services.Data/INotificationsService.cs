using System.Collections.Generic;
using System.Threading.Tasks;

using AYN.Web.ViewModels.Notifications;

namespace AYN.Services.Data
{
    public interface INotificationsService
    {
        Task CreateAsync(string text, string redirectUrl, string toUserId);

        Task<IEnumerable<T>> GetAll<T>(string userId);

        int GetCount(string userId);

        Task MarkAsRead(string notificationId);

        Task MarkAllAsRead(string userId);
    }
}
