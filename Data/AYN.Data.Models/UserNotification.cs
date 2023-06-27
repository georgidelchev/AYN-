using System;
using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;

namespace AYN.Data.Models;

public class UserNotification : BaseDeletableModel<string>
{
    public UserNotification()
    {
        this.Id = Guid.NewGuid().ToString();
    }

    [Required]
    public string ApplicationUserId { get; set; }

    public virtual ApplicationUser ApplicationUser { get; set; }

    [Required]
    public string NotificationId { get; set; }

    public virtual Notification Notification { get; set; }

    [Required]
    public bool IsRead { get; set; }
}
