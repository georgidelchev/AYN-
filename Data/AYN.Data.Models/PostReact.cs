using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;
using AYN.Data.Models.Enumerations;

namespace AYN.Data.Models;

public class PostReact : BaseDeletableModel<int>
{
    [Required]
    public int PostId { get; set; }

    public Post Post { get; set; }

    [Required]
    public string ApplicationUserId { get; set; }

    public ApplicationUser ApplicationUser { get; set; }

    [Required]
    public ReactionType ReactionType { get; set; }
}
