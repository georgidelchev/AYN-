using System.Collections.Generic;
using System.Threading.Tasks;

namespace AYN.Services.Data
{
    public interface INotificationsService
    {
        Task<IEnumerable<T>> GetAll<T>(string userId);

        int GetCount(string userId);

        Task MarkAsRead(string notificationId);

        Task MarkAllAsRead(string userId);
    }
}
