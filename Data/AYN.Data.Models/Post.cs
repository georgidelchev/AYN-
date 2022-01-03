using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;

using static AYN.Common.AttributeConstraints;

namespace AYN.Data.Models;

public class Post : BaseDeletableModel<int>
{
    [Required]
    [MaxLength(PostTitleMaxLength)]
    public string Title { get; set; }

    [Required]
    [MaxLength(PostContentMaxLength)]
    public string Content { get; set; }

    [Required]
    public string AddedByUserId { get; set; }

    public virtual ApplicationUser ApplicationUser { get; set; }

    public virtual ICollection<Tag> Tags { get; set; }
        = new HashSet<Tag>();

    public virtual ICollection<PostVote> PostVotes { get; set; }
        = new HashSet<PostVote>();

    public virtual ICollection<PostReact> PostReacts { get; set; }
        = new HashSet<PostReact>();
}
