using System.Collections.Generic;
using System.Threading.Tasks;

namespace AYN.Services.Data.Interfaces;

public interface INotificationsService
{
    Task CreateAsync(string text, string redirectUrl, string toUserId);

    Task<IEnumerable<T>> GetAll<T>(string userId);

    int GetCount(string userId);

    Task MarkAsRead(string notificationId);

    Task MarkAllAsRead(string userId);
}
