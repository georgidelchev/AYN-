using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace AYN.Services.Data
{
    public class NotificationsService : INotificationsService
    {
        private readonly IDeletableEntityRepository<UserNotification> userNotificationsRepository;

        public NotificationsService(
            IDeletableEntityRepository<UserNotification> userNotificationsRepository)
        {
            this.userNotificationsRepository = userNotificationsRepository;
        }

        public async Task<IEnumerable<T>> GetAll<T>(string userId)
        {
            var notifications = await this.userNotificationsRepository
                .All()
                .Where(n => n.ApplicationUserId == userId)
                .Select(n => n.Notification)
                .OrderByDescending(n => n.CreatedOn)
                .To<T>()
                .ToListAsync();

            return notifications;
        }

        public int GetCount(string userId)
        {
            var notificationsCount = this.userNotificationsRepository
                .All()
                .Count(n => n.ApplicationUserId == userId);

            return notificationsCount;
        }

        public async Task MarkAsRead(string notificationId)
        {
            var notification = this.userNotificationsRepository
                .All()
                .FirstOrDefault(n => n.NotificationId == notificationId);

            notification.IsRead = true;

            this.userNotificationsRepository.Update(notification);
            await this.userNotificationsRepository.SaveChangesAsync();
        }

        public async Task MarkAllAsRead(string userId)
        {
            var notifications = await this.userNotificationsRepository
                .All()
                .Where(n => n.ApplicationUserId == userId)
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.IsRead = true;

                this.userNotificationsRepository.Update(notification);
                await this.userNotificationsRepository.SaveChangesAsync();
            }
        }
    }
}
