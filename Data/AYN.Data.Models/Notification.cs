using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;

using static AYN.Common.AttributeConstraints;

namespace AYN.Data.Models;

public class Notification : BaseDeletableModel<string>
{
    public Notification()
    {
        this.Id = Guid.NewGuid().ToString();
    }

    [Required]
    [MaxLength(NotificationTextMaxLength)]
    public string Text { get; set; }

    [Required]
    public string RedirectUrl { get; set; }

    public ICollection<ApplicationUser> Users { get; set; }
        = new HashSet<ApplicationUser>();
}
