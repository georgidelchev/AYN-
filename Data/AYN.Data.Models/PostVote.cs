using System;
using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;

namespace AYN.Data.Models;

public class PostVote : BaseModel<string>
{
    public PostVote()
    {
        this.Id = Guid.NewGuid().ToString();
    }

    [Required]
    public int PostId { get; set; }

    public Post Post { get; set; }

    [Required]
    public string ApplicationUserId { get; set; }

    public ApplicationUser ApplicationUser { get; set; }

    [Required]
    public decimal Value { get; set; }
}
