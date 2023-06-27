using System;
using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;

namespace AYN.Data.Models;

public class FollowerFollowee : BaseDeletableModel<string>
{
    public FollowerFollowee()
    {
        this.Id = Guid.NewGuid().ToString();
    }

    [Required]
    public string FollowerId { get; set; }

    public virtual ApplicationUser Follower { get; set; }

    [Required]
    public string FolloweeId { get; set; }

    public virtual ApplicationUser Followee { get; set; }
}
