using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Data.Interfaces;
using AYN.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace AYN.Services.Data.Implementations;

public class NotificationsService : INotificationsService
{
    private readonly IDeletableEntityRepository<UserNotification> userNotificationsRepository;
    private readonly IDeletableEntityRepository<Notification> notificationsRepository;

    public NotificationsService(
        IDeletableEntityRepository<UserNotification> userNotificationsRepository,
        IDeletableEntityRepository<Notification> notificationsRepository)
    {
        this.userNotificationsRepository = userNotificationsRepository;
        this.notificationsRepository = notificationsRepository;
    }

    public async Task CreateAsync(string text, string redirectUrl, string toUserId)
    {
        var notification = new Notification()
        {
            RedirectUrl = redirectUrl,
            Text = text,
        };

        await this.notificationsRepository.AddAsync(notification);
        await this.notificationsRepository.SaveChangesAsync();

        await this.NotifyAsync(notification.Id, toUserId);
    }

    public async Task<IEnumerable<T>> GetAll<T>(string userId)
    {
        var notifications = await this.userNotificationsRepository
            .All()
            .Where(n => n.ApplicationUserId == userId && !n.IsRead)
            .Select(n => n.Notification)
            .OrderByDescending(n => n.CreatedOn)
            .To<T>()
            .ToListAsync();

        return notifications;
    }

    public int GetCount(string userId)
        => this.userNotificationsRepository
            .All()
            .Count(n => n.ApplicationUserId == userId && !n.IsRead);

    public async Task MarkAsRead(string notificationId)
    {
        var notification = await this.userNotificationsRepository
            .All()
            .SingleOrDefaultAsync(n => n.NotificationId == notificationId);

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

    // Helper method
    private async Task NotifyAsync(string notificationId, string toUserId)
    {
        var userNotification = new UserNotification()
        {
            ApplicationUserId = toUserId,
            IsRead = false,
            NotificationId = notificationId,
        };

        await this.userNotificationsRepository.AddAsync(userNotification);
        await this.userNotificationsRepository.SaveChangesAsync();
    }
}
