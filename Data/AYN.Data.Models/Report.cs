using System;
using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;
using AYN.Data.Models.Enumerations;

using static AYN.Common.AttributeConstraints;

namespace AYN.Data.Models;

public class Report : BaseDeletableModel<string>
{
    public Report()
    {
        this.Id = Guid.NewGuid().ToString();
    }

    [Required]
    [MaxLength(ReportContentMaxLength)]
    public string Description { get; set; }

    [Required]
    public ReportType ReportType { get; set; }

    [Required]
    public string ReportingUserId { get; set; }

    public virtual ApplicationUser ReportingUser { get; set; }

    [Required]
    public string ReportedUserId { get; set; }

    public virtual ApplicationUser ReportedUser { get; set; }

    [Required]
    public string ReportedAdId { get; set; }

    public virtual Ad ReportedAd { get; set; }
}
